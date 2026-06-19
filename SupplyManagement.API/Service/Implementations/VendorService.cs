using SupplyManagement.API.Data.Repositories;
using SupplyManagement.API.DTO;
using SupplyManagement.API.Models;
using SupplyManagement.API.Service.Interfaces;

namespace SupplyManagement.API.Service.Implementations
{
    public class VendorService : IVendorService
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly IRepository<VendorProfile> _profileRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IFileService _fileService;

        public VendorService(
            ICompanyRepository companyRepo,
            IRepository<VendorProfile> profileRepo,
            IRepository<User> userRepo,
            IFileService fileService)
        {
            _companyRepo = companyRepo;
            _profileRepo = profileRepo;
            _userRepo = userRepo;
            _fileService = fileService;
        }

        private static CompanyResponseDto MapToDto(Company c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            PhotoPath = c.PhotoPath,
            RegistrationStatus = c.RegistrationStatus.ToString(),
            VendorStatus = c.VendorStatus.ToString(),
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            VendorProfile = c.VendorProfile == null ? null : new VendorProfileResponseDto
            {
                Id = c.VendorProfile.Id,
                CompanyId = c.VendorProfile.CompanyId,
                BusinessSector = c.VendorProfile.BusinessSector,
                CompanyType = c.VendorProfile.CompanyType,
                CreatedAt = c.VendorProfile.CreatedAt,
                UpdatedAt = c.VendorProfile.UpdatedAt,
            }
        };

        public async Task<CompanyResponseDto> RegisterCompanyAsync(RegisterCompanyDto dto)
        {
            var existingUsers = await _userRepo.GetAllAsync();
            if (existingUsers.Any(u => u.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Username sudah digunakan.");

            string photoPath = string.Empty;
            if (dto.Photo != null)
                photoPath = await _fileService.UploadFileAsync(dto.Photo, "photos");

            var company = new Company
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PhotoPath = photoPath,
                RegistrationStatus = RegistrationStatus.Pending,
                VendorStatus = VendorStatus.NotVendor
            };
            await _companyRepo.AddAsync(company);

            var user = new User
            {
                Id = company.Id,
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Vendor"
            };
            await _userRepo.AddAsync(user);

            await _companyRepo.SaveChangesAsync();

            return MapToDto(company);
        }

        public async Task<VendorProfileResponseDto> CompleteVendorProfileAsync(Guid companyId, CompleteVendorProfileDto dto)
        {
            var company = await _companyRepo.GetWithProfileAsync(companyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            if (company.RegistrationStatus != RegistrationStatus.ApprovedByAdmin)
                throw new InvalidOperationException("Registration has not been approved by Admin yet.");

            if (company.VendorProfile != null)
            {
                company.VendorProfile.BusinessSector = dto.BusinessSector;
                company.VendorProfile.CompanyType = dto.CompanyType;
                company.VendorProfile.UpdatedAt = DateTime.UtcNow;
                company.VendorStatus = VendorStatus.ProfileSubmitted;
                company.UpdatedAt = DateTime.UtcNow;

                _companyRepo.Update(company);
                await _companyRepo.SaveChangesAsync();

                return new VendorProfileResponseDto
                {
                    Id = company.VendorProfile.Id,
                    CompanyId = company.VendorProfile.CompanyId,
                    BusinessSector = company.VendorProfile.BusinessSector,
                    CompanyType = company.VendorProfile.CompanyType,
                    CreatedAt = company.VendorProfile.CreatedAt,
                    UpdatedAt = company.VendorProfile.UpdatedAt,
                };
            }

            var profile = new VendorProfile
            {
                CompanyId = companyId,
                BusinessSector = dto.BusinessSector,
                CompanyType = dto.CompanyType
            };

            await _profileRepo.AddAsync(profile);

            company.VendorStatus = VendorStatus.ProfileSubmitted;
            company.UpdatedAt = DateTime.UtcNow;
            _companyRepo.Update(company);
            await _companyRepo.SaveChangesAsync();

            return new VendorProfileResponseDto
            {
                Id = profile.Id,
                CompanyId = profile.CompanyId,
                BusinessSector = profile.BusinessSector,
                CompanyType = profile.CompanyType,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt,
            };
        }

        public async Task ApproveCompanyRegistrationAsync(Guid companyId, bool isApproved)
        {
            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            if (company.RegistrationStatus != RegistrationStatus.Pending)
                throw new InvalidOperationException("Company registration is not in Pending state.");

            company.RegistrationStatus = isApproved ? RegistrationStatus.ApprovedByAdmin : RegistrationStatus.Rejected;
            company.VendorStatus = isApproved ? VendorStatus.PendingProfileCompletion : VendorStatus.Rejected;
            company.UpdatedAt = DateTime.UtcNow;

            _companyRepo.Update(company);
            await _companyRepo.SaveChangesAsync();
        }

        public async Task ApproveVendorProfileAsync(Guid companyId, bool isApproved)
        {
            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            if (company.VendorStatus != VendorStatus.ProfileSubmitted)
                throw new InvalidOperationException("Vendor profile is not in ProfileSubmitted state.");

            company.VendorStatus = isApproved
                ? VendorStatus.ApprovedByManager
                : VendorStatus.PendingProfileCompletion;
            company.UpdatedAt = DateTime.UtcNow;

            _companyRepo.Update(company);
            await _companyRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepo.GetAllAsync();
            return companies.Select(MapToDto);
        }

        public async Task<CompanyResponseDto?> GetCompanyByIdAsync(Guid id)
        {
            var company = await _companyRepo.GetWithProfileAsync(id);
            return company == null ? null : MapToDto(company);
        }

        public async Task<CompanyResponseDto> UpdateCompanyAsync(Guid id, UpdateCompanyDto dto)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            company.Name = dto.Name;
            company.Email = dto.Email;
            company.PhoneNumber = dto.PhoneNumber;

            if (dto.Photo != null)
                company.PhotoPath = await _fileService.UploadFileAsync(dto.Photo, "photos");

            company.UpdatedAt = DateTime.UtcNow;

            _companyRepo.Update(company);
            await _companyRepo.SaveChangesAsync();

            return MapToDto(company);
        }

        public async Task DeleteCompanyAsync(Guid id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            _companyRepo.Delete(company);
            await _companyRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetPendingRegistrationsAsync()
        {
            var companies = await _companyRepo.GetByRegistrationStatusAsync(RegistrationStatus.Pending);
            return companies.Select(MapToDto);
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetPendingVendorProfilesAsync()
        {
            var companies = await _companyRepo.GetByVendorStatusAsync(VendorStatus.ProfileSubmitted);
            return companies
                .Where(c => c.RegistrationStatus == RegistrationStatus.ApprovedByAdmin)
                .Select(MapToDto);
        }

        public async Task SubmitTenderAsync(SubmitTenderDto dto)
        {
            var company = await _companyRepo.GetByIdAsync(dto.CompanyId);
            if (company == null)
                throw new KeyNotFoundException("Company not found.");

            if (company.VendorStatus != VendorStatus.ApprovedByManager)
                throw new InvalidOperationException(
                    $"Vendor belum disetujui. Status saat ini: {company.VendorStatus}. " +
                    "Hanya vendor dengan status 'ApprovedByManager' yang dapat mengikuti proyek.");

            await Task.CompletedTask;
        }
    }
}

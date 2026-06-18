using SupplyManagement.API.DTO;

namespace SupplyManagement.API.Service.Interfaces
{
    public interface IVendorService
    {
        Task<CompanyResponseDto> RegisterCompanyAsync(RegisterCompanyDto dto);
        Task<VendorProfileResponseDto> CompleteVendorProfileAsync(Guid companyId, CompleteVendorProfileDto dto);

        Task ApproveCompanyRegistrationAsync(Guid companyId, bool isApproved);
        Task ApproveVendorProfileAsync(Guid companyId, bool isApproved);

        Task<IEnumerable<CompanyResponseDto>> GetAllCompaniesAsync();
        Task<CompanyResponseDto?> GetCompanyByIdAsync(Guid id);
        Task<CompanyResponseDto> UpdateCompanyAsync(Guid id, UpdateCompanyDto dto);
        Task DeleteCompanyAsync(Guid id);

        Task<IEnumerable<CompanyResponseDto>> GetPendingRegistrationsAsync();
        Task<IEnumerable<CompanyResponseDto>> GetPendingVendorProfilesAsync();
        Task SubmitTenderAsync(SubmitTenderDto dto);
    }
}

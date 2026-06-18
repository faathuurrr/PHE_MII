using SupplyManagement.API.Models;

namespace SupplyManagement.API.DTO
{
    // ─── Auth ───────────────────────────────────────────────────────────────

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    // ─── Company / Registration ──────────────────────────────────────────────

    public class RegisterCompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
    }

    public class UpdateCompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
    }

    public class CompanyResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public string RegistrationStatus { get; set; } = string.Empty;
        public string VendorStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public VendorProfileResponseDto? VendorProfile { get; set; }
    }

    // ─── Vendor Profile ──────────────────────────────────────────────────────

    public class CompleteVendorProfileDto
    {
        public Guid CompanyId { get; set; }
        public string BusinessSector { get; set; } = string.Empty;
        public string CompanyType { get; set; } = string.Empty;
    }

    public class UpdateVendorProfileDto
    {
        public string BusinessSector { get; set; } = string.Empty;
        public string CompanyType { get; set; } = string.Empty;
    }

    public class VendorProfileResponseDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string BusinessSector { get; set; } = string.Empty;
        public string CompanyType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // ─── Tender ─────────────────────────────────────────────────────────────

    public class SubmitTenderDto
    {
        public Guid CompanyId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

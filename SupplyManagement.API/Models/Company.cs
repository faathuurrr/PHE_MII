namespace SupplyManagement.API.Models
{
    public enum RegistrationStatus
    {
        Pending,
        ApprovedByAdmin,
        Rejected
    }

    public enum VendorStatus
    {
        NotVendor,
        PendingProfileCompletion,
        ProfileSubmitted,
        ApprovedByManager,
        Rejected
    }

    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Pending;
        public VendorStatus VendorStatus { get; set; } = VendorStatus.NotVendor;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public VendorProfile? VendorProfile { get; set; }
    }
}

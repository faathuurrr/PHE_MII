namespace SupplyManagement.API.Models
{
    public class VendorProfile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CompanyId { get; set; }
        
        public string BusinessSector { get; set; } = string.Empty;
        public string CompanyType { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
    }
}

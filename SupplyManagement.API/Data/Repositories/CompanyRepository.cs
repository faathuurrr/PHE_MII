using SupplyManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace SupplyManagement.API.Data.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetWithProfileAsync(Guid id);
        Task<IEnumerable<Company>> GetByRegistrationStatusAsync(RegistrationStatus status);
        Task<IEnumerable<Company>> GetByVendorStatusAsync(VendorStatus status);
    }

    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Company?> GetWithProfileAsync(Guid id)
        {
            return await _dbSet.Include(c => c.VendorProfile).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Company>> GetByRegistrationStatusAsync(RegistrationStatus status)
        {
            return await _dbSet.Where(c => c.RegistrationStatus == status).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetByVendorStatusAsync(VendorStatus status)
        {
            return await _dbSet.Include(c => c.VendorProfile).Where(c => c.VendorStatus == status).ToListAsync();
        }
    }
}

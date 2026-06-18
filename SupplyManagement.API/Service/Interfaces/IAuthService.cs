using SupplyManagement.API.DTO;

namespace SupplyManagement.API.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto);
        Task RegisterAdminOrManagerAsync(string username, string password, string role);
    }
}

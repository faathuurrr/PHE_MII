using Microsoft.AspNetCore.Mvc;
using SupplyManagement.API.DTO;
using SupplyManagement.API.Service.Interfaces;

namespace SupplyManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _authService.AuthenticateAsync(loginDto);
            if (response == null) return Unauthorized(new { message = "Invalid username or password" });

            return Ok(response);
        }

        [HttpPost("register-staff")]
        public async Task<IActionResult> RegisterStaff([FromBody] LoginDto loginDto, [FromQuery] string role)
        {
            try
            {
                await _authService.RegisterAdminOrManagerAsync(loginDto.Username, loginDto.Password, role);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

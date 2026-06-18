using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplyManagement.API.Service.Interfaces;

namespace SupplyManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApprovalController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public ApprovalController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet("admin/pending-registrations")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingRegistrations()
        {
            var companies = await _vendorService.GetPendingRegistrationsAsync();
            return Ok(companies);
        }

        [HttpPost("admin/approve/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminApprove(Guid id, [FromQuery] bool approve)
        {
            try
            {
                await _vendorService.ApproveCompanyRegistrationAsync(id, approve);
                return Ok(new { message = $"Registration {(approve ? "approved" : "rejected")} by Admin." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("manager/pending-profiles")]
        [Authorize(Roles = "LogisticManager")]
        public async Task<IActionResult> GetPendingProfiles()
        {
            var companies = await _vendorService.GetPendingVendorProfilesAsync();
            return Ok(companies);
        }

        [HttpPost("manager/approve/{id:guid}")]
        [Authorize(Roles = "LogisticManager")]
        public async Task<IActionResult> ManagerApprove(Guid id, [FromQuery] bool approve)
        {
            try
            {
                await _vendorService.ApproveVendorProfileAsync(id, approve);
                var msg = approve
                    ? "Vendor profile approved. Vendor is now active."
                    : "Vendor profile rejected. Company can re-submit the profile.";
                return Ok(new { message = msg });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

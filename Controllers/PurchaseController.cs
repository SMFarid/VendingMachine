using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.DTOs;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Buyer")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        [Route("Buy")]
        public async Task<ActionResult<PurchaseResponse>> Buy(PurchaseRequest request)
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var response = await _purchaseService.PurchaseProductAsync(userId!, request);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("Deposit")]
        public async Task<ActionResult<PurchaseResponse>> Deposit(PurchaseRequest request)
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var response = await _purchaseService.PurchaseProductAsync(userId!, request);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { error = ex.Message });
            }
        }
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioService.Application.Interfaces;

namespace PortfolioService.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PortfolioController(IPortfolioService portfolioService) : ControllerBase
    {
        private readonly IPortfolioService _portfolioService = portfolioService;

        private Guid GetUserIdFromToken()
        {
            var userIdString = User.FindFirst("sub")?.Value
                               ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
                throw new UnauthorizedAccessException("Lack of Id in JWT token.");
            return Guid.Parse(userIdString);
        }
        [HttpGet("stocks")]
        public async Task<IActionResult> GetStocks()
        {
            var userId = GetUserIdFromToken();
            var stocks = await _portfolioService.GetStocks(userId);
            return Ok(stocks);
        }

        [HttpPost("stocks/buy")]
        public async Task<IActionResult> BuyStocks([FromBody] StockOperationRequest request)
        {
            var userId = GetUserIdFromToken();
            var result = await _portfolioService.AddStocks(userId, request.StockId, request.Quantity);
            return Ok(result);
        }

        [HttpPost("stocks/sell")]
        public async Task<IActionResult> SellStocks([FromBody] StockOperationRequest request)
        {
            var userId = GetUserIdFromToken();
            var result = await _portfolioService.RemoveStocks(userId, request.StockId, request.Quantity);
            return Ok(result);
        }

        public record StockOperationRequest(Guid StockId, int Quantity);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioService.Application.Interfaces;
using PortfolioService.Domain.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortfolioService.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private Guid GetUserIdFromToken()
        {
            var userIdString = User.FindFirst("sub")?.Value
                               ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
                throw new UnauthorizedAccessException("Brak identyfikatora użytkownika w tokenie.");
            return Guid.Parse(userIdString);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount()
        {
            var userId = GetUserIdFromToken();
            var result = await _accountService.CreateAccount(userId);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = GetUserIdFromToken();
            var result = await _accountService.DeleteAccount(userId);
            return Ok(result);
        }

        [HttpPost("deposits")]
        public async Task<IActionResult> Deposit([FromQuery] decimal amount)
        {
            var userId = GetUserIdFromToken();
            var result = await _accountService.Deposit(userId, amount);
            return Ok(result);
        }

        [HttpPost("withdrawals")]
        public async Task<IActionResult> Withdraw([FromQuery] decimal amount)
        {
            var userId = GetUserIdFromToken();
            var result = await _accountService.Withdraw(userId, amount);
            return Ok(result);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetUserIdFromToken();
            var balance = await _accountService.GetBalance(userId);
            return Ok(balance);
        }

        [HttpGet("reserved-balance")]
        public async Task<IActionResult> GetReservedBalance()
        {
            var userId = GetUserIdFromToken();
            var reservedBalance = await _accountService.GetReservedBalance(userId);
            return Ok(reservedBalance);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = GetUserIdFromToken();
            var userInfo = await _accountService.GetUserInfo(userId);
            return Ok(userInfo);
        }

        [HttpGet("stocks")]
        public async Task<IActionResult> GetStocks()
        {
            var userId = GetUserIdFromToken();
            var stocks = await _accountService.GetStocks(userId);
            return Ok(stocks);
        }

        [HttpPost("stocks/buy")]
        public async Task<IActionResult> BuyStocks([FromBody] StockOperationRequest request)
        {
            var userId = GetUserIdFromToken();
            var result = await _accountService.AddStocks(userId, request.StockID, request.Quantity);
            return Ok(result);
        }

        [HttpPost("stocks/sell")]
        public async Task<IActionResult> SellStocks([FromBody] StockOperationRequest request)
        {
            var userId = GetUserIdFromToken();
            var result = await _accountService.RemoveStocks(userId, request.StockID, request.Quantity);
            return Ok(result);
        }

        public record StockOperationRequest(Guid StockID, int Quantity);
    }
}

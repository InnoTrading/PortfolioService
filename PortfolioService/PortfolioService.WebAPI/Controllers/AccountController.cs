using Microsoft.AspNetCore.Mvc;
using PortfolioService.Application.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PortfolioService.Application.DTOs;

namespace PortfolioService.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService  = accountService;

        private string GetUserIdFromToken()
        {
            var userId= User.FindFirst("sub")?.Value
                               ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("Lack of Id in JWT token.");
            return userId;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] RegisterDto registerDto)
        {
            var result = await _accountService.CreateAccount(registerDto);
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
            var result = await _accountService.Withdrawal(userId, amount);
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
    }
}

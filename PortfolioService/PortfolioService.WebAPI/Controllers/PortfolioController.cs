using Microsoft.AspNetCore.Mvc;
using PortfolioService.Application.Interfaces;

namespace PortfolioService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("{userId}/create")]
        public async Task<IActionResult> CreateAccount([FromRoute] Guid userId)
        {
            var result = await _accountService.CreateAccount(userId);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid userId)
        {
            var result = await _accountService.DeleteAccount(userId);
            return Ok(result);
        }

        [HttpPost("{userId}/deposit")]
        public async Task<IActionResult> Deposit([FromRoute] Guid userId, [FromQuery] decimal amount)
        {
            var result = await _accountService.Deposit(userId, amount);
            return Ok(result);
        }

        [HttpPost("{userId}/withdraw")]
        public async Task<IActionResult> Withdraw([FromRoute] Guid userId, [FromQuery] decimal amount)
        {
            var result = await _accountService.Withdraw(userId, amount);
            return Ok(result);
        }

        [HttpGet("{userId}/balance")]
        public async Task<IActionResult> GetBalance([FromRoute] Guid userId)
        {
            var balance = await _accountService.GetBalance(userId);
            return Ok(balance);
        }

        [HttpGet("{userId}/reservedbalance")]
        public async Task<IActionResult> GetReservedBalance([FromRoute] Guid userId)
        {
            var reservedBalance = await _accountService.GetReservedBalance(userId);
            return Ok(reservedBalance);
        }

        [HttpGet("{userId}/userinfo")]
        public async Task<IActionResult> GetUserInfo([FromRoute] Guid userId)
        {
            var userInfo = await _accountService.GetUserInfo(userId);
            return Ok(userInfo);
        }

        [HttpGet("{userId}/stocks")]
        public async Task<IActionResult> GetStocks([FromRoute] Guid userId)
        {
            var stocks = await _accountService.GetStocks(userId);
            return Ok(stocks);
        }
    }
}

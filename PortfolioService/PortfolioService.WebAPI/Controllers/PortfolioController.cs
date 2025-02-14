using Microsoft.AspNetCore.Mvc;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController(IAccountManager accountService) : Controller
    {
        private readonly IAccountManager _accountService = accountService;
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("balance/userID")]
        public ActionResult<decimal> GetBalance(Guid userID)
        {
            var balance = _accountService.Balance(userID);
            
            return Ok(balance);
        }
    }
}

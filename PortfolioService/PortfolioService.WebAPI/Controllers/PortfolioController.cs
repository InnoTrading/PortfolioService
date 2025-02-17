using Microsoft.AspNetCore.Mvc;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController(IAccountManager accountService) : Controller
    {
        private readonly IAccountManager _accountService = accountService;

        
    }
}

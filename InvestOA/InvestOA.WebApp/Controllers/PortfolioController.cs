using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestOA.WebApp.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("quote")]
        public IActionResult Quote()
        {
            return View();
        }

        [HttpGet]
        [Route("buy")]
        public IActionResult Buy()
        {
            return View();
        }

        [HttpGet]
        [Route("sell")]
        public IActionResult Sell()
        {
            return View();
        }

        [HttpGet]
        [Route("history")]
        public IActionResult History()
        {
            return View();
        }
    }
}

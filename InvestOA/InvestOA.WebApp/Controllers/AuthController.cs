using Microsoft.AspNetCore.Mvc;

namespace InvestOA.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}

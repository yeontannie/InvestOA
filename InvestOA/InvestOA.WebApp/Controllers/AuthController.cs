using InvestOA.Core;
using InvestOA.Core.Requests;
using InvestOA.DataManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvestOA.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<User> userM, SignInManager<User> signInM,
            IConfiguration config)
        {
            userManager = userM;
            signInManager = signInM;
            configuration = config;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([Bind("Email,Password,Confirmation")] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var username = model.Email.Split('@');
                var userExists = await userManager.FindByNameAsync(username.First());
                if (userExists != null)
                {
                    return View("Apology", new Apology { StatusCode = 409, Description = "User already exists" });
                }

                User user = new()
                {
                    Email = model.Email,
                    UserName = username.First(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Cash = 10000
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return View("Apology", new Apology { StatusCode = 500, Description = "Can't create user" });
                }

                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Auth",
                    new { userName = user.UserName, code = code },
                    HttpContext.Request.Scheme);

                EmailService emailService = new EmailService();
                emailService.SendEmail(model.Email, "Confirm your account",
                    $"Your account has been created. To login you need to confirm " +
                    $"your accout first. To do that: <a href='{callbackUrl}'>Click here!</a>", configuration);


                return RedirectToAction("Confirmation");

                /*  Add to User Role
                    if (await roleManager.RoleExistsAsync(UserRoles.User))
                    {
                        await userManager.AddToRoleAsync(user, UserRoles.User);
                    }
                 */
            }
            return View("Apology", new Apology { StatusCode = 400, Description = "Data is not valid" });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("confirmation")]
        public IActionResult Confirmation()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string code)
        {
            if (userName == null || code == null)
            {
                return View("Error");
            }
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return View("Apology", new Apology { StatusCode = 409, Description = "User don't exist" });
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return RedirectToAction("Confirmed");
            }
            else
                return View("Error");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("confirmed")]
        public IActionResult Confirmed()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([Bind("Email,Password")] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var username = model.Email.Split('@');
                var user = await userManager.FindByNameAsync(username.First());
                if (user != null && user.EmailConfirmed == true)
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Portfolio");
                    }
                    else
                    {
                        return View("Apology", new Apology { StatusCode = 400, Description = "Wrong email or password. Try again" });
                    }
                }
                else
                {
                    return View("Apology", new Apology { StatusCode = 400, Description = "Your account is not confirmed yet." });
                }
            }
            return View("Apology", new Apology { StatusCode = 400, Description = "Data is not valid" });
        }

        [Authorize]
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction("Login");
        }
    }
}

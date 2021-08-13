using FilmCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FilmCatalog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<User> userManager, 
                                SignInManager<User> signInManager, 
                                ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = new User
            {
                Email = viewModel.Email,
                UserName = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                MiddleName = viewModel.MiddleName
            };

            var created = await userManager.CreateAsync(user, viewModel.Password);
            if (created.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                logger.LogInformation("user success signup");

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in created.Errors)
            {
                logger.LogError("user dont signup");
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(viewModel);

        }

        [HttpGet]
        public IActionResult SignIn(string returnUrl= null)
        {
            return View(new SignInViewModel { ReturnUrl = returnUrl });
        }

    }
}

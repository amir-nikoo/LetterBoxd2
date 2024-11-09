using LetterBoxd2.Data;
using LetterBoxd2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetterBoxd2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDBContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        [HttpGet("login")]
        public IActionResult LogInPage()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginPage", model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("GetAll", "Movie");
            }
            
            ModelState.AddModelError("", "Invalid username or password.");
            return View("LoginPage", model);
        }

        [HttpGet("signup")]
        public IActionResult SignUpPage()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View("SignUpPage", model);
            }

            if (!ModelState.IsValid)
            {
                return View("SignupPage", model);
            }

            var newUser = new User
            {
                UserName = model.Username,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);

                return RedirectToAction("GetAll", "Movie");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("SignUpPage", model);
            }
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LoginPage", "Account");
        }
    }
}
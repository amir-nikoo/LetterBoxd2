using LetterBoxd2.Data;
using LetterBoxd2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetterBoxd2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AccountController(ApplicationDBContext context)
        {
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
            if(!ModelState.IsValid)
            {
                return View("LoginPage", model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(s => s.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View("LoginPage", model);
            }

            return RedirectToAction("GetAll", "Movie");
        }

        [HttpGet("signup")]
        public IActionResult SignUpPage()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View("SignUpPage", model);
            }

            if(await _context.Users.AnyAsync(s => s.Username == model.Username))
            {
                ViewBag.ErrorMessage = "This username is taken.";
                ViewBag.Username = model.Username;
                return View("SignupPage");
            }

            var newUser = new User{
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };
            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetAll", "Movie");
        }
    }
}
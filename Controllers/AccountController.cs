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
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                ViewBag.Username = username;
                return View("LoginPage");
            }

            return RedirectToAction("GetAll", "Movie");
        }

        [HttpGet("signup")]
        public IActionResult SignUpPage()
        {
            return View();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(string username, string password, string confirmPassword)
        {
            if(password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords don't match.";
                ViewBag.Username = username;
                return View("SignupPage");
            }

            if(await _context.Users.AnyAsync(s => s.Username == username))
            {
                ViewBag.ErrorMessage = "This username is taken.";
                ViewBag.Username = username;
                return View("SignupPage");
            }

            var newUser = new User{
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetAll", "Movie");
        }
    }
}
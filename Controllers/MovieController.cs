using System.ComponentModel;
using LetterBoxd2.Data;
using LetterBoxd2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetterBoxd2.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDBContext _context;
        public MovieController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("movies")]
        public IActionResult GetAll()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        [HttpGet("movies/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movieTarget = await _context.Movies.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
            if(movieTarget == null)
            {
                return NotFound("The requested movie was not found.");
            }

            return View("GetById", movieTarget);
        }
    }
}
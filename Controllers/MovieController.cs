using System.ComponentModel;
using LetterBoxd2.Data;
using LetterBoxd2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace LetterBoxd2.Controllers
{
    [Authorize]
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
            ViewData["Username"] = User.Identity.Name;
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

            return View(movieTarget);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int movieId, string content)
        {
            if(content.Length > 50)
            {
                return BadRequest("Too many characters");
            }

            var newComment = new Comment{
                Username = User.Identity.Name,
                Content = content,
                MovieId = movieId
            };

            await _context.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetById", "Movie", new { id = movieId});
        }

        [HttpGet("movies/editcomment/{id:int}")]
        public async Task<IActionResult> EditCommentPage(int id)
        {
            var targetComment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
            if(targetComment == null)
            {
                return BadRequest("Comment not found.");
            }
            return View(targetComment);
        }

        [HttpPost("movies/editcomment/{id:int}")]
        public async Task<IActionResult> EditComment(int id, string content)
        {
            var targetComment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
            if(targetComment == null)
            {
                return BadRequest("Comment not found.");
            }

            targetComment.Content = content;
            await _context.SaveChangesAsync();
            return RedirectToAction("GetById", "Movie", new {id = targetComment.MovieId});
        }

        [HttpPost("movies/deletecomment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var targetComment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
            if(targetComment == null)
            {
                return BadRequest("Comment not found.");
            }

            _context.Remove(targetComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetById", "Movie", new {id = targetComment.MovieId});
        }
    }
}
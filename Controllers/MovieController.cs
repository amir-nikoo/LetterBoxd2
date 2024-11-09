using LetterBoxd2.Data;
using LetterBoxd2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View(movies);
        }

        [HttpGet("movies/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movieTarget = await _context.Movies.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
            if(movieTarget == null)
            {
                return NotFound();
            }

            return View(movieTarget);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentViewModel comment)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("GetById", new{id = comment.MovieId});
            }

            var newComment = new Comment{
                Username = User.Identity.Name,
                Content = comment.Content,
                MovieId = comment.MovieId
            };

            await _context.AddAsync(newComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetById", "Movie", new { id = comment.MovieId});
        }

        [HttpGet("movies/editcomment/{id:int}")]
        public async Task<IActionResult> EditCommentPage(int id)
        {
            var targetComment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
            if(targetComment == null)
            {
                return NotFound();
            }
            return View(targetComment);
        }

        [HttpPost("movies/editcomment/{id:int}")]
        public async Task<IActionResult> EditComment(int id, string content)
        {
            var targetComment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
            if(targetComment == null)
            {
                return NotFound();
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
                return NotFound();
            }

            _context.Remove(targetComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetById", "Movie", new {id = targetComment.MovieId});
        }
    }
}
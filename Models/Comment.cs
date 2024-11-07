using System.ComponentModel.DataAnnotations;

namespace LetterBoxd2.Models
{
    public class Comment
    {
        public int Id { get;set; }
        public string UserName { get;set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(50, ErrorMessage = "Comment cannot exceed 50 characters.")]
        public string Content { get;set; }
        public int MovieId { get;set; }
        public Movie Movie { get;set; }
    }
}
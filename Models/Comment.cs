namespace LetterBoxd2.Models
{
    public class Comment
    {
        public int Id { get;set; }
        public string Username { get;set; }
        public string Content { get;set; }
        public int MovieId { get;set; }
        public Movie Movie { get;set; }
    }
}
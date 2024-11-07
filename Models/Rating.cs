namespace LetterBoxd2.Models
{
    public class Rating
    {
        public int Id { get;set; }
        public int MovieId { get;set; }
        public int Score { get;set; }
        public Movie Movie { get;set; }
    }
}
namespace LetterBoxd2.Models
{
    public class Movie
    {
        public int Id { get;set; }
        public string Title { get;set; }
        public string Genre { get;set; }
        public int ReleaseYear { get;set; }
        public List<Comment> Comments { get;set; } = new List<Comment>();
        public string PosterUrl { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public double AverageRating 
        {
            get
            {
                if (Ratings.Count == 0) return 0;
                return Ratings.Average(r => r.Score);
            }
        }

        public void AddRating(int rating)
        {
            Ratings.Add(new Rating { MovieId = this.Id, Score = rating });
        }

    }
}
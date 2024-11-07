namespace LetterBoxd2.Models
{
    public class Movie
    {
        public int Id { get;set; }
        public string Title { get;set; }
        public string Genre { get;set; }
        public int ReleaseYear { get;set; }
        public List<Comment> Comments { get;set; } = new List<Comment>();
        public double AverageRating { get;private set; } = 0.0;
        public int NumberOfRatings { get;private set; } = 0;
        public string PosterUrl { get; set; }

        public void AddRating(double rating)
        {
            AverageRating = (AverageRating * NumberOfRatings + rating) / (NumberOfRatings + 1);
            
            NumberOfRatings++;
        }

    }
}
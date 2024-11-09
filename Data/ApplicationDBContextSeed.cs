using LetterBoxd2.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetterBoxd2.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedMoviesAsync(ApplicationDBContext context)
        {
            if (!context.Movies.Any())
            {
                var movies = new List<Movie>
                {
                    new Movie
                    {
                        Title = "The Shawshank Redemption",
                        Genre = "Drama",
                        ReleaseYear = 1994,
                        PosterUrl = "https://media-cache.cinematerial.com/p/500x/b5v2e9jg/the-shawshank-redemption-movie-poster.jpg"
                    },
                    new Movie
                    {
                        Title = "The Godfather",
                        Genre = "Crime",
                        ReleaseYear = 1972,
                        PosterUrl = "https://media-cache.cinematerial.com/p/500x/id2w2swk/the-godfather-movie-poster.jpg"
                    },
                    new Movie
                    {
                        Title = "The Dark Knight",
                        Genre = "Action",
                        ReleaseYear = 2008,
                        PosterUrl = "https://media-cache.cinematerial.com/p/500x/udapnxr3/the-dark-knight-movie-poster.jpg"
                    }
                };
                
                context.Movies.AddRange(movies);
                await context.SaveChangesAsync();
            }
        }
    }
}

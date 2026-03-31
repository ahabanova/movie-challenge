using MovieChallenge.API.DTOs.Categories;
using MovieChallenge.API.DTOs.Movies;

namespace MovieChallenge.API.DTOs.UserMovieEntries
{
    public class CreateUserMovieEntryDto
    {
        public int MovieId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Rating { get; set; }
    }
}

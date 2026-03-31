using MovieChallenge.API.DTOs.Categories;
using MovieChallenge.API.DTOs.Movies;
using MovieChallenge.API.Models;

namespace MovieChallenge.API.DTOs.UserMovieEntries
{
    public class UserMovieEntryDto
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public MovieDto Movie { get; set; } = null!;
        public CategoryDto Category { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public int? Rating { get; set; }
    }
}
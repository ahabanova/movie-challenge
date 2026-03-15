using MovieChallenge.API.Models;

namespace MovieChallenge.API.DTOs.Categories
{
    public class CreateCategoryDto
    {
        public string Assignment { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}

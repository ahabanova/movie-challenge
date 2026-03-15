using MovieChallenge.API.DTOs.Categories;

namespace MovieChallenge.API.DTOs.Challenges
{
    public class ChallengeDetailDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}

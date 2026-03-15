using MovieChallenge.API.DTOs.Challenges;
using MovieChallenge.API.Models;

namespace MovieChallenge.API.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public string Assignment { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}

using MovieChallenge.API.DTOs.Categories;
using MovieChallenge.API.Models;

namespace MovieChallenge.API.DTOs.Challenges
{
    public class ChallengeDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
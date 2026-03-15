namespace MovieChallenge.API.DTOs.Challenges
{
    public class CreateChallengeDto
    {
        public int Year { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

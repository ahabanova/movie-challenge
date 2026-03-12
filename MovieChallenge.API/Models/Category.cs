namespace MovieChallenge.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public Challenge Challenge { get; set; } = null!;
        public int ChallengeId { get; set; }

        public string Assignment { get; set; } = string.Empty;

        public int Order { get; set; }

        ICollection<UserMovieEntry> UserMovieEntries { get; set; } = new List<UserMovieEntry>();
    }
}

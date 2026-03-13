namespace MovieChallenge.API.Models
{
    public class UserMovieEntry
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public AppUser User { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public Movie Movie { get; set; } = null!;
        public int MovieId { get; set; }
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Rating { get; set; }
    }
}
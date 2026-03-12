namespace MovieChallenge.API.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public int TmdbId { get; set; }
        public string OriginalTitle { get; set; } = string.Empty;
        public string? EnglishTitle { get; set; } = string.Empty;
        public string? CzechTitle { get; set; } = string.Empty;
        public int Year { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
        public string? Overview { get; set; } = string.Empty;
    }
}

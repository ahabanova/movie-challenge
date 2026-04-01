namespace MovieChallenge.API.DTOs.UserMovieEntries
{
    public class UpdateUserMovieEntryDto
    {
        public int TmdbId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? Rating { get; set; }
    }
}

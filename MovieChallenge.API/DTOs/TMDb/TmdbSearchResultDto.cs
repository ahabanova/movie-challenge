using System.Text.Json.Serialization;

namespace MovieChallenge.API.DTOs.TMDb
{
    public class TmdbSearchResultDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; } = null!;

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = null!;

        [JsonPropertyName("overview")]
        public string Overview { get; set; } = null!;

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = null!;
    }
}
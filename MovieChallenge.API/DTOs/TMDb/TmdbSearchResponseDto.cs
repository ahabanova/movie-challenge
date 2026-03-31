using System.Text.Json.Serialization;

namespace MovieChallenge.API.DTOs.TMDb
{
    public class TmdbSearchResponseDto
    {
        [JsonPropertyName("results")]
        public List<TmdbSearchResultDto> Results { get; set; } = new List<TmdbSearchResultDto>();
    }
}
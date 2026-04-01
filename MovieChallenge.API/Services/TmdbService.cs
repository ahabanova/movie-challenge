using MovieChallenge.API.DTOs.TMDb;

namespace MovieChallenge.API.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Tmdb:ApiKey"]!;
        }

        public async Task<List<TmdbSearchResultDto>> SearchMoviesAsync(string movieName)
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/search/movie?query={movieName}&language=cs&api_key={_apiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TmdbSearchResponseDto>();

            return result?.Results ?? new List<TmdbSearchResultDto>();
        }
    }
}
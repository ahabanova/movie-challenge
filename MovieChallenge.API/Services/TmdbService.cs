using Microsoft.EntityFrameworkCore;
using MovieChallenge.API.Data;
using MovieChallenge.API.DTOs.TMDb;
using MovieChallenge.API.Models;
using static System.Net.WebRequestMethods;

namespace MovieChallenge.API.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly AppDbContext _db;
      
        public TmdbService(HttpClient httpClient, IConfiguration configuration, AppDbContext db )
        {
            _httpClient = httpClient;
            _apiKey = configuration["Tmdb:ApiKey"]!;
            _db = db;
        }

        public async Task<List<TmdbSearchResultDto>> SearchMoviesAsync(string movieName)
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/search/movie?query={movieName}&language=cs&api_key={_apiKey}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TmdbSearchResponseDto>();

            return result?.Results ?? new List<TmdbSearchResultDto>();
        }

        public async Task<int> GetMovieDetailsAsync(int tmdbId)
        {
            var movie = await _db.Movies.FirstOrDefaultAsync(m => m.TmdbId == tmdbId);
            if (movie != null) return movie.Id;

            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{tmdbId}?language=cs&api_key={_apiKey}");
            if (!response.IsSuccessStatusCode) return 0;

            var result = await response.Content.ReadFromJsonAsync<TmdbSearchResultDto>();
            if (result == null) return 0;

            movie = new Movie
            {
                TmdbId = result.Id,
                CzechTitle = result.Title,
                OriginalTitle = result.OriginalTitle,
                Year = DateTime.TryParse(result.ReleaseDate, out var date) ? date.Year : 0,
                Overview = result.Overview,
                PosterUrl = $"https://image.tmdb.org/t/p/w500{result.PosterPath}"
            };

            _db.Movies.Add(movie);
            await _db.SaveChangesAsync();

            return movie.Id;
        }
    }
}
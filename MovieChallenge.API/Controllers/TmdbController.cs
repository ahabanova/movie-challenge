using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieChallenge.API.Data;
using MovieChallenge.API.Services;

namespace MovieChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TmdbController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly TmdbService _tmdbService;

        public TmdbController(AppDbContext db, TmdbService tmdbService)
        {
            _db = db;
            _tmdbService = tmdbService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return BadRequest("Query cannot be empty");
            var results = await _tmdbService.SearchMoviesAsync(query);
            return Ok(results);
        }
    }
}

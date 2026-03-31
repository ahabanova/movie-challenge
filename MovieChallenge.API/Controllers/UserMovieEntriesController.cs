using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieChallenge.API.Data;
using MovieChallenge.API.DTOs.Categories;
using MovieChallenge.API.DTOs.UserMovieEntries;
using MovieChallenge.API.DTOs.Movies;
using MovieChallenge.API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MovieChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMovieEntriesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserMovieEntriesController(AppDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet("~/api/challenges/{challengeId}/entries")]
        public async Task<IActionResult> GetUserMovieEntries(int challengeId)
        {
            if (!await _db.Challenges.AnyAsync(c => c.Id == challengeId)) return NotFound("Výzva nenalezena");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return NotFound("Uživatel nenalezen");

            var entries = await _db.UserMovieEntries
                .Where(e => e.UserId == userId && e.Category.ChallengeId == challengeId)
                .Select(e => new UserMovieEntryDto
                {
                    Id = e.Id,
                    Category = new CategoryDto
                    {
                        Id = e.Category.Id,
                        ChallengeId = e.Category.ChallengeId,
                        Assignment = e.Category.Assignment,
                        Order = e.Category.Order
                    },
                    DateAdded = e.DateAdded,
                    DateUpdated = e.DateUpdated,
                    Description = e.Description,
                    Movie = new MovieDto
                    {
                        Id = e.Movie.Id,
                        CzechTitle = e.Movie.CzechTitle,
                        EnglishTitle = e.Movie.EnglishTitle,
                        OriginalTitle = e.Movie.OriginalTitle,
                        Overview = e.Movie.Overview,
                        PosterUrl = e.Movie.PosterUrl,
                        TmdbId = e.Movie.TmdbId,
                        Year = e.Movie.Year
                    },
                    Rating = e.Rating,
                })
                .ToListAsync();

            return Ok(entries);
        }

        [Authorize]
        [HttpPost("~/api/challenges/{challengeId}/entries")]
        public async Task<IActionResult> CreateUserMovieEntry(int challengeId, CreateUserMovieEntryDto data)
        {
            if (!await _db.Challenges.AnyAsync(c => c.Id == challengeId)) return NotFound("Výzva nenalezena");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return NotFound("Uživatel nenalezen");

            if (await _db.UserMovieEntries.AnyAsync(e => e.UserId == userId && e.CategoryId == data.CategoryId))
                return BadRequest("Pro tuto kategorii již máte přidaný film");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("Uživatel nenalezen");

            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == data.CategoryId);
            if (category == null) return NotFound("Kategorie nenalezena");

            var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == data.MovieId);
            if (movie == null)
            {
                movie = new Movie
                {
                    //TODO z tmdb
                };
                _db.Movies.Add(movie);
            }

            var entry = new UserMovieEntry
            {
                Category = category,
                CategoryId = data.CategoryId,
                DateAdded = DateTime.UtcNow,
                Description = data.Description,
                Movie = movie,
                MovieId = data.MovieId,
                Rating = data.Rating,
                UserId = userId,
                User = user,
            };

            _db.UserMovieEntries.Add(entry);
            await _db.SaveChangesAsync();

            var entryDto = new UserMovieEntryDto
            {
                Id = entry.Id,
                Category = new CategoryDto
                {
                    Id = category.Id,
                    ChallengeId = category.ChallengeId,
                    Assignment = category.Assignment,
                    Order = category.Order
                },
                DateAdded = entry.DateAdded,
                Description = entry.Description,
                Movie = new MovieDto
                {
                    Id = movie.Id,
                    CzechTitle = movie.CzechTitle,
                    EnglishTitle = movie.EnglishTitle,
                    OriginalTitle = movie.OriginalTitle,
                    Overview = movie.Overview,
                    PosterUrl = movie.PosterUrl,
                    TmdbId = movie.TmdbId,
                    Year = movie.Year
                },
                Rating = entry.Rating
            };

            return Ok(entryDto);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserMovieEntry(int id, UpdateUserMovieEntryDto data)
        {
            if (data == null) return BadRequest("Neplatná data");

            var entry = await _db.UserMovieEntries
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (entry == null) return NotFound("Záznam nenalezen");

            if (entry.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value) return Forbid();

            var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == data.MovieId);
            if (movie == null)
            {
                movie = new Movie
                {
                    //TODO z tmdb
                };
                _db.Movies.Add(movie);
            }

            entry.MovieId = data.MovieId;
            entry.Movie = movie;
            entry.Description = data.Description;
            entry.Rating = data.Rating;
            entry.DateUpdated = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var entryDto = new UserMovieEntryDto
            {
                Id = entry.Id,
                Category = new CategoryDto
                {
                    Id = entry.Category.Id,
                    ChallengeId = entry.Category.ChallengeId,
                    Assignment = entry.Category.Assignment,
                    Order = entry.Category.Order
                },
                DateAdded = entry.DateAdded,
                Description = entry.Description,
                Movie = new MovieDto
                {
                    Id = movie.Id,
                    CzechTitle = movie.CzechTitle,
                    EnglishTitle = movie.EnglishTitle,
                    OriginalTitle = movie.OriginalTitle,
                    Overview = movie.Overview,
                    PosterUrl = movie.PosterUrl,
                    TmdbId = movie.TmdbId,
                    Year = movie.Year
                },
                Rating = entry.Rating
            };

            return Ok(entryDto);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserMovieEntry(int id)
        {
            var entry = await _db.UserMovieEntries.FindAsync(id);
            if (entry == null) return NotFound("Záznam nenalezen");

            if (entry.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value) return Forbid();

            _db.UserMovieEntries.Remove(entry);
            await _db.SaveChangesAsync();
            return Ok("Záznam byl smazán");
        }
    }
}

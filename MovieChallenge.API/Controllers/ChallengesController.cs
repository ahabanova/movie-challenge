using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieChallenge.API.Data;
using MovieChallenge.API.DTOs.Auth;
using MovieChallenge.API.DTOs.Challenges;
using MovieChallenge.API.Models;

namespace MovieChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ChallengesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetChallenges()
        {
            var challenges = await _db.Challenges.ToListAsync();
            return Ok(challenges);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChallenge(int id)
        {
            var challenge = await _db.Challenges
                .Include(ch => ch.Categories)
                .FirstOrDefaultAsync(ch => ch.Id == id);
            if (challenge == null) return NotFound("Výzva nenalezena");
            return Ok(challenge);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateChallenge(CreateChallengeDto data)
        {
            if (data == null) return BadRequest("Neplatná data");
            var challenge = new Challenge
            {
                Year = data.Year,
                Title = data.Title,
                IsActive = data.IsActive
            };
            _db.Challenges.Add(challenge);
            await _db.SaveChangesAsync();
            return Ok(challenge);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateChallenge(int id, UpdateChallengeDto data)
        {
            if (data == null) return BadRequest("Neplatná data");
            var challenge = await _db.Challenges.FindAsync(id);
            if (challenge == null) return NotFound("Výzva nenalezena");
            challenge.Year = data.Year;
            challenge.Title = data.Title;
            challenge.IsActive = data.IsActive;
            await _db.SaveChangesAsync();
            return Ok(challenge);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            var challenge = await _db.Challenges.FindAsync(id);
            if (challenge == null) return NotFound("Výzva nenalezena");
            _db.Challenges.Remove(challenge);
            await _db.SaveChangesAsync();
            return Ok("Výzva smazána");
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieChallenge.API.Data;
using MovieChallenge.API.DTOs.Categories;
using MovieChallenge.API.Models;

namespace MovieChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CategoriesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("~/api/challenges/{challengeId}/categories")]
        public async Task<IActionResult> GetCategories(int challengeId)
        {
            if (!await _db.Challenges.AnyAsync(c => c.Id == challengeId)) return NotFound("Výzva nenalezena");

            var categories = await _db.Categories
                .Where(c => c.ChallengeId == challengeId)
                .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        ChallengeId = c.ChallengeId,
                        Assignment = c.Assignment,
                        Order = c.Order
                    })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpPost("~/api/challenges/{challengeId}/categories")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(int challengeId, CreateCategoryDto data)
        {
            if (!await _db.Challenges.AnyAsync(c => c.Id == challengeId)) return NotFound("Výzva nenalezena");

            if (data == null) return BadRequest("Neplatná data");

            var category = new Category
            {
                ChallengeId = challengeId,
                Assignment = data.Assignment,
                Order = data.Order
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                ChallengeId = category.ChallengeId,
                Assignment = category.Assignment,
                Order = category.Order
            };
            return Ok(categoryDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto data)
        {
            if (data == null) return BadRequest("Neplatná data");

            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound("Kategorie nenalezena");
            category.Assignment = data.Assignment;
            category.Order = data.Order;

            await _db.SaveChangesAsync();

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                ChallengeId = category.ChallengeId,
                Assignment = category.Assignment,
                Order = category.Order
            };
            return Ok(categoryDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {   var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound("Kategorie nenalezena");

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return Ok("Kategorie byla smazána");
        }
    }
}


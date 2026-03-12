using Microsoft.AspNetCore.Identity;

namespace MovieChallenge.API.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}

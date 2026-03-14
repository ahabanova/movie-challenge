using Microsoft.AspNetCore.Identity;

namespace MovieChallenge.API.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<UserMovieEntry> UserMovieEntries { get; set; } = new List<UserMovieEntry>();
        public string Name { get; set; } = string.Empty;
    }
}

using Microsoft.EntityFrameworkCore;
using MovieChallenge.API.Models;

namespace MovieChallenge.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserMovieEntry> UserMovieEntries { get; set; }
    }
}
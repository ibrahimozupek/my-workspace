using Microsoft.EntityFrameworkCore;

namespace GeziRehberi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) { }

        public DbSet<City> Cities { get; set; } 
        public DbSet<Place> Places { get; set; }
        public DbSet<Comment> comments { get; set; }

    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Place> Places { get; set; }
        public ICollection<Comment> comments { get; set; }

    }
    public class Place
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public City City { get; set; }
    }
    public class Comment
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public City City { get; set; } 
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fall2025_Project3_jppierre.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fall2025_Project3_jppierre.Models.Movie> Movie { get; set; } = default!;
        public DbSet<Fall2025_Project3_jppierre.Models.Actor> Actor { get; set; } = default!;
    }
}

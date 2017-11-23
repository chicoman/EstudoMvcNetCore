using Microsoft.EntityFrameworkCore;

namespace Web.Models
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Web.Models.Movie> Movie { get; set; }
    }
}
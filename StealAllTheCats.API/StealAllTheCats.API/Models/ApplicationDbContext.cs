using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace StealAllTheCats.API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CatEntity> CatEntities { get; set; }
        public DbSet<TagEntity> TagEntities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}

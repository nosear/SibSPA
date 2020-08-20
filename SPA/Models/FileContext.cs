using Microsoft.EntityFrameworkCore;

namespace SPA.Models
{
    public class FilesContext : DbContext
    {
        public DbSet<ModFile> Files { get; set; }
        public DbSet<ModBlob> ModBlobs { get; set; }
        public FilesContext(DbContextOptions<FilesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}

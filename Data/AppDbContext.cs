using Microsoft.EntityFrameworkCore;
using UVS.Models;

namespace UVS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GeneratedData> Logs => Set<GeneratedData>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=JuniorTaskDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

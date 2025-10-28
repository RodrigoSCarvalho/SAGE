using Microsoft.EntityFrameworkCore;
using SAGE.Domain.ChangePlans;

namespace SAGE.Infrastructure.Persistence.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ChangePlan> ChangePlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}

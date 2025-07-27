using Microsoft.EntityFrameworkCore;
using SAGE.Domain.ChangePlans;

namespace SAGE.Infrastructure.Persistence.Context
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

    public DbSet<ChangePlan> ChangePlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
  }
}

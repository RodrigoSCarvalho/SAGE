using Microsoft.EntityFrameworkCore;
using SAGE.Domain.ChangePlans;
using SAGE.Domain.Interfaces;
using SAGE.Infrastructure.Persistence.Context;

namespace SAGE.Infrastructure.Persistence.Repositories
{
  public class ChangePlanRepository(AppDbContext context) : IChangePlanRepository
  {
    private readonly AppDbContext _context = context;

    public async Task AddAsync(ChangePlan plan)
    {
      _context.ChangePlans.Add(plan);
      await _context.SaveChangesAsync();
    }

    public async Task<ChangePlan?> GetByIdAsync(Guid id)
    {
      return await _context.ChangePlans
        .AsNoTracking()
        .FirstOrDefaultAsync(cp => cp.Id == id);
    }
  }
}
using Microsoft.EntityFrameworkCore;
using SAGE.Domain.ChangePlans;
using SAGE.Infrastructure.Persistence.Context;

namespace SAGE.Infrastructure.Persistence.Repositories
{
    public class ChangePlanRepository(AppDbContext context) : IChangePlanRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(ChangePlan plan, CancellationToken cancellationToken = default)
        {
            _context.ChangePlans.Add(plan);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ChangePlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.ChangePlans
              .AsNoTracking()
              .FirstOrDefaultAsync(cp => cp.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(ChangePlan plan, CancellationToken cancellationToken = default)
        {
            _context.ChangePlans.Update(plan);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var plan = await _context.ChangePlans.FindAsync(id);
            if (plan != null)
            {
                _context.ChangePlans.Remove(plan);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.ChangePlans.CountAsync();
        }
    }
}
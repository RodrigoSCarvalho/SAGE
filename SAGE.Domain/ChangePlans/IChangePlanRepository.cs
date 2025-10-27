namespace SAGE.Domain.ChangePlans
{
    public interface IChangePlanRepository
    {
        Task AddAsync(ChangePlan plan, CancellationToken cancellationToken);
        Task UpdateAsync(ChangePlan plan, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ChangePlan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<int> CountAsync();
    }
}
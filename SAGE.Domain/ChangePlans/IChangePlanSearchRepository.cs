namespace SAGE.Domain.ChangePlans;
public interface IChangePlanSearchRepository
{
    Task IndexAsync(ChangePlanDocument document, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ChangePlanDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ChangePlanDocument>> SearchAsync(
        string searchTerm, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<List<ChangePlanDocument>> SearchWithFilterAsync(
        string searchTerm, DateTime? createdAfter, DateTime? createdBefore, List<string>? tags, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}
using SAGE.Domain.ChangePlans;

namespace SAGE.Domain.Interfaces
{
  public interface IChangePlanRepository
  {
    Task AddAsync(ChangePlan plan);
  }
}
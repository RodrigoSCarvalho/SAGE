using MediatR;
using SAGE.Domain.ChangePlans;
using SAGE.Domain.Interfaces;


namespace SAGE.Application.ChangePlans.Commands;

public class CreateChangePlanCommandHandler(IChangePlanRepository repository) : IRequestHandler<CreateChangePlanCommand, Guid>
{
  private readonly IChangePlanRepository _repository = repository;

  public async Task<Guid> Handle(CreateChangePlanCommand request, CancellationToken cancellationToken)
  {
    var plan = new ChangePlan(request.Title, request.Description, request.CreatedBy);
    await _repository.AddAsync(plan);
    return plan.Id;
  }
}
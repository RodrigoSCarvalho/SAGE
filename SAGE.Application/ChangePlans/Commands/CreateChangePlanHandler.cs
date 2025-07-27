using MediatR;
using SAGE.Domain.ChangePlans;
using SAGE.Infrastructure.Persistence;
using SAGE.Infrastructure.Persistence.Context;

namespace SAGE.Application.ChangePlans.Commands;

public class CreateChangePlanCommandHandler(AppDbContext context) : IRequestHandler<CreateChangePlanCommand, Guid>
{
  private readonly AppDbContext _context = context;
  public async Task<Guid> Handle(CreateChangePlanCommand request, CancellationToken cancellationToken)
  {
    var ChangePlan = new ChangePlan(request.Title, request.Description, request.CreatedBy);
    _context.ChangePlans.Add(ChangePlan);
    await _context.SaveChangesAsync(cancellationToken);

    return ChangePlan.Id;
  }
}
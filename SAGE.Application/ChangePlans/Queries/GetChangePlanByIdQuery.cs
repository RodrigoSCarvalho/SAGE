using MediatR;
using SAGE.Application.ChangePlans.DTOs;

namespace SAGE.Application.ChangePlans.Queries
{
  public class GetChangePlanByIdQuery(Guid id) : IRequest<ChangePlanDto>
  {
    public Guid Id { get; set; } = id;
  }
}
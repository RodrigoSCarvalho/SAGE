using MediatR;
using SAGE.Application.ChangePlans.DTOs;

namespace SAGE.Application.ChangePlans.Queries.GetChangePlanById;
  public record GetChangePlanByIdQuery(Guid Id) : IRequest<ChangePlanResponseDto?>
  {
  }

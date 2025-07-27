using System.Net;
using MediatR;

namespace SAGE.Application.ChangePlans.Commands;

public class CreateChangePlanCommand : IRequest<Guid>
{
  public string Title { get; set; } = "";
  public string Description { get; set; } = "";
  public Guid CreatedBy { get; set; }
}
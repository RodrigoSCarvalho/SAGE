using System.Net;
using MediatR;
using SAGE.Application.ChangePlans.DTOs;

namespace SAGE.Application.ChangePlans.Commands;

public class CreateChangePlanCommand(ChangePlanDto request) : IRequest<Guid>
{
  public string Title { get; set; } = request.Title;
  public string Description { get; set; } = request.Description;
  public Guid CreatedBy { get; set; } = request.CreatedBy;
}
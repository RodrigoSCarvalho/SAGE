using MediatR;
using SAGE.Application.ChangePlans.DTOs;

namespace SAGE.Application.ChangePlans.Commands.CreateChangePlan;

public record CreateChangePlanCommand(string Title, string Description, Guid CreatedBy) : IRequest<ChangePlanResponseDto>
{
    public string Title { get; set; } = Title;
    public string Description { get; set; } = Description;
    public Guid CreatedBy { get; set; } = CreatedBy;
}
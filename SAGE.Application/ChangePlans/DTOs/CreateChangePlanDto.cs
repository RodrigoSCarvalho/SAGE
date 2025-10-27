namespace SAGE.Application.ChangePlans.DTOs;

public class CreateChangePlanDto
{
    public String Title { get; set; } = string.Empty;
    public String Description { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
}
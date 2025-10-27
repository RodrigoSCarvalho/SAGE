namespace SAGE.Application.ChangePlans.DTOs;

public class ChangePlanResponseDto
{
  public Guid Id { get; set; }
  public String Title { get; set; } = string.Empty;
  public String Description { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public Guid CreatedBy { get; set; }

}
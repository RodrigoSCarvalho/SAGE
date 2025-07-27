using SAGE.Domain.Common;

namespace SAGE.Domain.ChangePlans;

public class ChangePlan : AggregateRoot<Guid>
{
  public string Title { get; private set; }
  public string Description { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public Guid CreatedBy { get; private set; }

  private ChangePlan()
  {
    Title = string.Empty;
    Description = string.Empty;
  }

  public ChangePlan(string title, string description, Guid createdBy)
  {
    Id = Guid.NewGuid();
    Title = title;
    Description = description;
    CreatedAt = DateTime.UtcNow;
    CreatedBy = createdBy;
  }

  public void Update(string title, string description)
  {
    Title = title;
    Description = description;
  }

}
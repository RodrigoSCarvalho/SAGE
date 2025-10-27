namespace SAGE.Domain.ChangePlans;

public class ChangePlanDocument
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    public string SearchText { get; set; } = string.Empty;
    public string Status { get; set; } = "active";
    public List<string> Tags { get; set; } = new();
}


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
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título não pode ser vazio.", nameof(title));

        if (title.Length > 255)
            throw new ArgumentException("Título deve ter no máximo 255 caracteres.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição não pode ser vazia.", nameof(description));

        if (createdBy == Guid.Empty)
            throw new ArgumentException("Cirador inválido.", nameof(createdBy));

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void Update(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título não pode ser vazio.", nameof(title));

        if (title.Length > 255)
            throw new ArgumentException("Título deve ter no máximo 255 caracteres.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição não pode ser vazia.", nameof(description));

        Title = title;
        Description = description;
    }

}
using FluentValidation;

namespace SAGE.Application.ChangePlans.Commands.CreateChangePlan;

public class CreateChangePlanValidator : AbstractValidator<CreateChangePlanCommand>
{
  public CreateChangePlanValidator()
  {
    RuleFor(x => x.Title)
      .NotEmpty().WithMessage("O título é obrigatório.")
      .MaximumLength(255).WithMessage("O título deve ter no máximo 255 caráctres.");

    RuleFor(x => x.Description)
      .NotEmpty().WithMessage("A descrição é obrigatória.");

    RuleFor(x => x.CreatedBy)
      .NotNull().NotEqual(Guid.Empty).WithMessage("O campo CreatedBy deve ser um GUID válido.");
  }
}
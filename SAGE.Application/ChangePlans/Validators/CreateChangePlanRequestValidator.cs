using System.Data;
using FluentValidation;
using SAGE.Application.ChangePlans.DTOs;

namespace SAGE.Application.ChangePlans.Validators;

public class CreateChangePlanRequestValidator : AbstractValidator<CreateChangePlanRequest>
{
  public CreateChangePlanRequestValidator()
  {
    RuleFor(x => x.Title)
      .NotEmpty().WithMessage("O título é obrigatório.")
      .MaximumLength(255).WithMessage("O título deve ter no máximo 255 caráctres.");

    RuleFor(x => x.Description)
      .NotEmpty().WithMessage("A descrição é obrigatória.");

    RuleFor(x => x.CreatedBy)
      .NotEqual(Guid.Empty).WithMessage("O campo CreatedBy deve ser um GUID válido.");
  }
}
using MediatR;
using SAGE.Application.ChangePlans.DTOs;

namespace SAGE.Application.ChangePlans.Queries.SearchChangePlans;

public record SearchChangePlansQuery(
    string? SearchTerm = null,
    DateTime? CreatedAfter = null,
    DateTime? CreatedBefore = null,
    List<string>? Tags = null,
    int Page = 1,
    int PageSize = 10) : IRequest<SearchResultDto<ChangePlanResponseDto>>;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.ChangePlans;

namespace SAGE.Application.ChangePlans.Queries.SearchChangePlans;

public class SearchChangePlansQueryHandler : IRequestHandler<SearchChangePlansQuery, SearchResultDto<ChangePlanResponseDto>>
{
    private readonly IChangePlanSearchRepository _searchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchChangePlansQueryHandler> _logger;

    public SearchChangePlansQueryHandler(IChangePlanSearchRepository searchRepository, IMapper mapper, ILogger<SearchChangePlansQueryHandler> logger)
    {
        _searchRepository = searchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SearchResultDto<ChangePlanResponseDto>> Handle(SearchChangePlansQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando planos: Term={SearchTerm}, Page={Page}, PageSize={PageSize}", request.SearchTerm, request.Page, request.PageSize);

        var documents = await _searchRepository.SearchWithFilterAsync(
            request.SearchTerm,
            request.CreatedAfter,
            request.CreatedBefore,
            request.Tags,
            request.Page,
            request.PageSize,
            cancellationToken);

        var items = _mapper.Map<List<ChangePlanResponseDto>>(documents);

        return new SearchResultDto<ChangePlanResponseDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = items.Count
        };
    }
}


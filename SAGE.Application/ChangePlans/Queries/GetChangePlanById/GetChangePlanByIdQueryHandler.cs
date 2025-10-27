using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.ChangePlans;

namespace SAGE.Application.ChangePlans.Queries.GetChangePlanById;
public class GetChangePlanByIdQueryHandler : IRequestHandler<GetChangePlanByIdQuery, ChangePlanResponseDto>
{
    private readonly IChangePlanSearchRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetChangePlanByIdQueryHandler> _logger;
    public GetChangePlanByIdQueryHandler(IChangePlanSearchRepository repository, IMapper mapper, ILogger<GetChangePlanByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ChangePlanResponseDto?> Handle(GetChangePlanByIdQuery request, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Buscando plano {Id} no Elasticsearch", request.Id);
        var document = await _repository.GetByIdAsync(request.Id);
        if (document == null)
        {
            _logger.LogWarning("Plano {Id} não encontrado.", request.Id);
            return null;
        }

        return _mapper.Map<ChangePlanResponseDto>(document);
    }
}
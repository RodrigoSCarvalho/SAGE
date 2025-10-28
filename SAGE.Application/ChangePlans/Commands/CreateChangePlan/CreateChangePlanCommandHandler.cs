using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.ChangePlans;

namespace SAGE.Application.ChangePlans.Commands.CreateChangePlan;

public class CreateChangePlanCommandHandler(IChangePlanRepository sqlRepository, IChangePlanSearchRepository searchRepository, IMapper mapper, ILogger<CreateChangePlanCommandHandler> logger) : IRequestHandler<CreateChangePlanCommand, ChangePlanResponseDto>
{
    private readonly IChangePlanRepository _sqlRepository = sqlRepository;
    private readonly IChangePlanSearchRepository _searchRepository = searchRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CreateChangePlanCommandHandler> _logger = logger;

    public async Task<ChangePlanResponseDto> Handle(CreateChangePlanCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Criando novo plano de mudança: {Title}", request.Title);
        var plan = new ChangePlan(request.Title, request.Description, request.CreatedBy);
        await _sqlRepository.AddAsync(plan, cancellationToken);
        _logger.LogInformation("Plano de mudança {Id} criado com sucesso no banco de dados.", plan.Id);

        try
        {
            var document = new ChangePlanDocument
            {
                Id = plan.Id,
                Title = plan.Title,
                Description = plan.Description,
                CreatedBy = plan.CreatedBy,
                CreatedAt = plan.CreatedAt
            };

            await _searchRepository.IndexAsync(document, cancellationToken);
            _logger.LogInformation("Plano de mudança {Id} indexado com sucesso no Elasticsearch.", plan.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao indexar plano de mudança {Id} no Elasticsearch.", plan.Id);
        }
        return _mapper.Map<ChangePlanResponseDto>(plan);
    }
}
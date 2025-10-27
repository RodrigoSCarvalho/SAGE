using AutoMapper;
using MediatR;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.ChangePlans;

namespace SAGE.Application.ChangePlans.Commands.CreateChangePlan;

public class CreateChangePlanCommandHandler(IChangePlanRepository sqlRepository, IChangePlanSearchRepository searchRepository, IMapper mapper) : IRequestHandler<CreateChangePlanCommand, ChangePlanResponseDto>
{
    private readonly IChangePlanRepository _sqlRepository = sqlRepository;
    private readonly IChangePlanSearchRepository _searchRepository = searchRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ChangePlanResponseDto> Handle(CreateChangePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new ChangePlan(request.Title, request.Description, request.CreatedBy);
        await _sqlRepository.AddAsync(plan, cancellationToken);

        var document = new ChangePlanDocument
        {
            Id = plan.Id,
            Title = plan.Title,
            Description = plan.Description,
            CreatedBy = plan.CreatedBy,
            CreatedAt = plan.CreatedAt
        };

        await _searchRepository.IndexAsync(document, cancellationToken);
        return _mapper.Map<ChangePlanResponseDto>(plan);
    }
}
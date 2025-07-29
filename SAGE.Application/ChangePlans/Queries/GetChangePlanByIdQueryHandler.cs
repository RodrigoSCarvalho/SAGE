using AutoMapper;
using MediatR;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.Exceptions;
using SAGE.Domain.Interfaces;

namespace SAGE.Application.ChangePlans.Queries
{
  public class GetChangePlanByIdQueryHandler : IRequestHandler<GetChangePlanByIdQuery, ChangePlanDto>
  {
    private readonly IChangePlanRepository _repository;
    private readonly IMapper _mapper;
    public GetChangePlanByIdQueryHandler(IChangePlanRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<ChangePlanDto> Handle(GetChangePlanByIdQuery request, CancellationToken cancellationToken)
    {
      var plan = await _repository.GetByIdAsync(request.Id);
      if (plan == null)
        throw new NotFoundException($"ChangePlan with id {request.Id} not found.");

      return _mapper.Map<ChangePlanDto>(plan);
    }
  }
}
using AutoMapper;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.ChangePlans;

namespace SAGE.Application.ChangePlans.Mappings;

public class ChangePlanProfile : Profile
{
    public ChangePlanProfile()
    {
        CreateMap<ChangePlan, ChangePlanResponseDto>();
        CreateMap<ChangePlanDocument, ChangePlanResponseDto>();
    }
}
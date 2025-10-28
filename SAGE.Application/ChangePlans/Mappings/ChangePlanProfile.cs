using AutoMapper;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.ChangePlans;

namespace SAGE.Application.ChangePlans.Mappings;

public class ChangePlanProfile : Profile
{
    public ChangePlanProfile()
    {
        CreateMap<ChangePlan, ChangePlanResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "ative"))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => new List<string>()));
        CreateMap<ChangePlanDocument, ChangePlanResponseDto>();
    }
}
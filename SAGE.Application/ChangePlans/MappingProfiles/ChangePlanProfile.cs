using AutoMapper;
using SAGE.Application.ChangePlans.DTOs;
using SAGE.Domain.ChangePlans;

namespace SAGE.Application.ChangePlans.MappingProfiles;

public class ChangePlanProfile : Profile
{
  public ChangePlanProfile()
  {
    CreateMap<ChangePlan, ChangePlanDto>();
  }
}
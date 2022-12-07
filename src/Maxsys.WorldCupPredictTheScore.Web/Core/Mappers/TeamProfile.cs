using AutoMapper;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Mappers;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<TeamDTO, TeamViewModel>()
            .ForMember(d => d.Id, cfg => cfg.MapFrom(s => s.Id))
            .ForMember(d => d.Name, cfg => cfg.MapFrom(s => s.Name))
            .ForMember(d => d.Code, cfg => cfg.MapFrom(s => s.Code))
            .ForMember(d => d.Flag, cfg => cfg.MapFrom(s => $"{s.Code}.webp"));
    }
}

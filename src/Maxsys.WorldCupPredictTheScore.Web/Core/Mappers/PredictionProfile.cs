using AutoMapper;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Mappers;

public class PredictionProfile : Profile
{
    public PredictionProfile()
    {
        CreateMap<MatchPredictionsItemDTO, PredictedScoreViewModel>()
            .ForMember(d => d.UserId, cfg => cfg.MapFrom(s => s.User.Id))
            .ForMember(d => d.UserName, cfg => cfg.MapFrom(s => s.User.Name))
            .ForMember(d => d.HomeTeamScore, cfg => cfg.MapFrom(s => s.HomeTeamScore.ToString()))
            .ForMember(d => d.AwayTeamScore, cfg => cfg.MapFrom(s => s.AwayTeamScore.ToString()))
            .ForMember(d => d.Points, cfg => cfg.MapFrom(s => default(int?)));

        CreateMap<MatchDTO, PredictListDTO>()
            .ForMember(d => d.MatchId, cfg => cfg.MapFrom(s => s.MatchId))
            .ForMember(d => d.Round, cfg => cfg.MapFrom(s => s.Round))
            .ForMember(d => d.MatchInfo, cfg => cfg.MapFrom(s => s.RoundToString()))
            .ForMember(d => d.Date, cfg => cfg.MapFrom(s => s.Date))
            .ForMember(d => d.HomeTeam, cfg => cfg.MapFrom(s => s.HomeTeam))
            .ForMember(d => d.AwayTeam, cfg => cfg.MapFrom(s => s.AwayTeam));

        CreateMap<PredictListDTO, PredictListViewModel>()
            .ForMember(d => d.MatchId, cfg => cfg.MapFrom(s => s.MatchId))
            .ForMember(d => d.MatchInfo, cfg => cfg.MapFrom(s => s.MatchInfo))
            .ForMember(d => d.Round, cfg => cfg.MapFrom(s => s.Round))
            .ForMember(d => d.Date, cfg => cfg.MapFrom(s => s.Date))
            .ForMember(d => d.HomeTeam, cfg => cfg.MapFrom(s => s.HomeTeam))
            .ForMember(d => d.AwayTeam, cfg => cfg.MapFrom(s => s.AwayTeam))
            .ForMember(d => d.HomeTeamScore, cfg => cfg.MapFrom(s => default(byte?)))
            .ForMember(d => d.AwayTeamScore, cfg => cfg.MapFrom(s => default(byte?)));

        }

}

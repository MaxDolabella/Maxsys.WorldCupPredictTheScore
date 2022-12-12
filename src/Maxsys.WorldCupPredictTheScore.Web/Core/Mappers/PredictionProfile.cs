using AutoMapper;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Prediction;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Mappers;

public class PredictionProfile : Profile
{
    public PredictionProfile()
    {
        CreateMap<MatchPredictionsItemDTO, PredictedScoreViewModelOld>()
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

        CreateMap<UserPredictionsItemDTO, UserPredictionsItemViewModel>();
        CreateMap<UserPredictionsDTO, UserPredictionsViewModel>();

        CreateMap<MatchPredictionsItemDTO, MatchPredictionsItemViewModel>();
        CreateMap<MatchPredictionsDTO, MatchPredictionsViewModel>()
            .ForMember(d => d.PreviousMatchId, cfg => cfg.MapFrom(s => default(Guid?)))
            .ForMember(d => d.NextMatchId, cfg => cfg.MapFrom(s => default(Guid?)));

        CreateMap<PreviousNextMatchDTO, MatchPredictionsViewModel>()
            .ForMember(d => d.PreviousMatchId, cfg => cfg.MapFrom(s => s.PreviousMatchId ))
            .ForMember(d => d.NextMatchId, cfg => cfg.MapFrom(s => s.NextMatchId));
    }
}
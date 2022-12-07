using AutoMapper;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Mappers;

public class MatchProfile : Profile
{
    public MatchProfile()
    {
        // ============================= DTO to VM =============================
        CreateMap<PreviousNextMatchDTO, MatchPredictionsViewModel>()
            .ForMember(d => d.PreviousMatchId, cfg => cfg.MapFrom(s => s.PreviousMatchId))
            .ForMember(d => d.NextMatchId, cfg => cfg.MapFrom(s => s.NextMatchId));

        CreateMap<MatchDTO, MatchViewModel>()
            .ForMember(d => d.MatchId, cfg => cfg.MapFrom(s => s.MatchId))
            .ForMember(d => d.Round, cfg => cfg.MapFrom(s => s.Round))
            .ForMember(d => d.MatchInfo, cfg => cfg.MapFrom(s => s.RoundToString()))
            .ForMember(d => d.Round, cfg => cfg.MapFrom(s => s.Round))
            .ForMember(d => d.Date, cfg => cfg.MapFrom(s => s.Date))
            .ForMember(d => d.HomeTeam, cfg => cfg.MapFrom(s => s.HomeTeam))
            .ForMember(d => d.AwayTeam, cfg => cfg.MapFrom(s => s.AwayTeam))
            .ForMember(d => d.HomeTeamScore, cfg => cfg.MapFrom(s => s.HomeTeamScore))
            .ForMember(d => d.AwayTeamScore, cfg => cfg.MapFrom(s => s.AwayTeamScore));

        CreateMap<MatchDTO, MatchEditViewModel>()
            .ForMember(d => d.MatchId, cfg => cfg.MapFrom(s => s.MatchId))
            .ForMember(d => d.HomeTeamName, cfg => cfg.MapFrom(s => s.HomeTeam.Name))
            .ForMember(d => d.AwayTeamName, cfg => cfg.MapFrom(s => s.AwayTeam.Name))
            .ForMember(d => d.HomeTeamScore, cfg => cfg.MapFrom(s => s.HomeTeamScore))
            .ForMember(d => d.AwayTeamScore, cfg => cfg.MapFrom(s => s.AwayTeamScore));

        CreateMap<MatchEditViewModel, MatchEditDTO>()
            .ForMember(d => d.MatchId, cfg => cfg.MapFrom(s => s.MatchId))
            .ForMember(d => d.HomeTeamScore, cfg => cfg.MapFrom(s => s.HomeTeamScore))
            .ForMember(d => d.AwayTeamScore, cfg => cfg.MapFrom(s => s.AwayTeamScore));

        /*
         var viewModel = new MatchCreateViewModel
        {
            Teams = teams,

            MatchDate = null,
            SelectedHomeTeamId = null,
            SelectedAwayTeamId = null
        };
        */
    }
}
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<MatchDTO> SelectMatch(this IQueryable<Match> query)
    {
        return query.Select(m => new MatchDTO
        {
            MatchId = m.Id,
            Group = m.Group,
            Round = m.Round,
            Date = m.Date,
            HomeTeam = new TeamDTO
            {
                Id = m.HomeTeam.Id,
                Name = m.HomeTeam.Name,
                Code = m.HomeTeam.Code
            },
            AwayTeam = new TeamDTO
            {
                Id = m.AwayTeam.Id,
                Name = m.AwayTeam.Name,
                Code = m.AwayTeam.Code
            },
            HomeTeamScore = m.HomeScore,
            AwayTeamScore = m.AwayScore
        });
    }

    public static IQueryable<TeamDTO> SelectTeam(this IQueryable<Team> query)
    {
        return query.Select(m => new TeamDTO
        {
            Id = m.Id,
            Name = m.Name,
            Code = m.Code
        });
    }
}
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;

namespace Microsoft.EntityFrameworkCore;

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
            HomeTeam = new TeamInfoDTO
            {
                Id = m.HomeTeam.Id,
                Name = m.HomeTeam.Name,
                Code = m.HomeTeam.Code
            },
            AwayTeam = new TeamInfoDTO
            {
                Id = m.AwayTeam.Id,
                Name = m.AwayTeam.Name,
                Code = m.AwayTeam.Code
            },
            HomeTeamScore = m.HomeScore,
            AwayTeamScore = m.AwayScore
        });
    }
}
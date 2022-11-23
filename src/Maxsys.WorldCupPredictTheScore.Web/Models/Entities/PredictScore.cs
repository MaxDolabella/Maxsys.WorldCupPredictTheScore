using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;

namespace Maxsys.WorldCupPredictTheScore.Web.Models.Entities;

/// <summary>
/// Palpite
/// </summary>
public sealed class PredictScore : Entity
{
    public PredictScore(Guid matchId, Guid userId, byte homeScore, byte awayScore)
        : base(Guid.NewGuid())
    {
        MatchId = matchId;
        UserId = userId;
        HomeTeamScore = homeScore;
        AwayTeamScore = awayScore;
    }

    protected PredictScore()
    { }

    public Guid MatchId { get; set; }
    public Guid UserId { get; set; }

    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }

    public Match Match { get; set; }
    public AppUser User { get; set; }

    public MatchWinner GetPredictWinner()
    {
        return HomeTeamScore > AwayTeamScore
            ? MatchWinner.HomeTeam
            : AwayTeamScore > HomeTeamScore
                ? MatchWinner.AwayTeam
                : MatchWinner.Draw;
    }
}
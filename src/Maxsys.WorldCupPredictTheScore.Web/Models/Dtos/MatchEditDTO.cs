namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class MatchEditDTO
{
    public Guid MatchId { get; set; }

    public byte? HomeTeamScore { get; set; }
    public byte? AwayTeamScore { get; set; }
}
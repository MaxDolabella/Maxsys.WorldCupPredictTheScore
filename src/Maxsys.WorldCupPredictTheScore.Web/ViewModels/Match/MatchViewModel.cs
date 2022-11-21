namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

public sealed class MatchViewModel
{
    public Guid MatchId { get; set; }
    public string MatchInfo { get; set; }
    public DateTime Date { get; set; }

    public TeamViewModel HomeTeam { get; set; }
    public TeamViewModel AwayTeam { get; set; }

    public byte? HomeTeamScore { get; set; }
    public byte? AwayTeamScore { get; set; }
}
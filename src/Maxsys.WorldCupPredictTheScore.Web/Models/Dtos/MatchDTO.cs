namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class MatchDTO
{
    public Guid MatchId { get; set; }
    public DateTime Date { get; set; }
    public char Group { get; set; }
    public byte Round { get; set; }

    public TeamDTO HomeTeam { get; set; }
    public TeamDTO AwayTeam { get; set; }

    public byte? HomeTeamScore { get; set; }
    public byte? AwayTeamScore { get; set; }
}
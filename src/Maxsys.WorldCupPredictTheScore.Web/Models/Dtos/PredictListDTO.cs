namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class PredictListDTO
{
    public Guid MatchId { get; set; }
    public byte Round { get; set; }
    public string MatchInfo { get; set; }
    public DateTime Date { get; set; }
    public TeamDTO HomeTeam { get; set; }
    public TeamDTO AwayTeam { get; set; }
}

/// <summary>
/// Contém Match
/// </summary>
public sealed class PredictedMatchDTO
{
    public MatchDTO Match { get; set; }
    public UserDTO User { get; set; }
    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }
}
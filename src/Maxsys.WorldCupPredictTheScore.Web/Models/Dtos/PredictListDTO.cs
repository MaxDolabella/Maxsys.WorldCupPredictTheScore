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
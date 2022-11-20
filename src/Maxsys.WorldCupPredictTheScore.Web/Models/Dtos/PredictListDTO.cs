namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class PredictListDTO
{
    public Guid MatchId { get; set; }
    public string MatchInfo { get; set; }
    public DateTime Date { get; set; }
    public TeamInfoDTO HomeTeam { get; set; }
    public TeamInfoDTO AwayTeam { get; set; }
}

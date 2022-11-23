namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class PredictPointsDTO
{
    public Guid UserId { get; set; }
    public Guid MatchId { get; set; }
    public Guid PredictId { get; set; }
    public int Points { get; set; }
}
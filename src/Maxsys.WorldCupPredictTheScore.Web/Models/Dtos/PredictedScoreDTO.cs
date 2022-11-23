namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class PredictedScoreDTO
{
    public Guid PredictionId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }
}
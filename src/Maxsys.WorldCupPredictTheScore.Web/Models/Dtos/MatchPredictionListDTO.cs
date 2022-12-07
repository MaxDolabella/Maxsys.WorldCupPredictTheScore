namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class MatchPredictionsDTO
{
    public MatchDTO Match { get; set; }
    public IReadOnlyList<PredictedScoreDTO> Predictions { get; set; }
}

public sealed class MatchPredictionsDTO_new
{
    public MatchDTO Match { get; set; }
    public IReadOnlyList<MatchPredictionsItemDTO> Predictions { get; set; }
}
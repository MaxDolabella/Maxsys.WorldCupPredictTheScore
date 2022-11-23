using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;

public sealed class MatchPredictionsViewModel
{
    public MatchViewModel Match { get; set; }
    public IReadOnlyList<PredictedScoreViewModel> Predictions { get; set; }
}

public sealed class PredictedScoreViewModel
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string HomeTeamScore { get; set; }
    public string AwayTeamScore { get; set; }
    public int? Points { get; set; } = null;
}
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;

public sealed class MatchPredictionsViewModel
{
    public MatchViewModel Match { get; set; }
    public IReadOnlyList<PredictedScoreViewModel> Predictions { get; set; }
}

public sealed class PredictedScoreViewModel
{
    public string UserName { get; set; }
    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }
}
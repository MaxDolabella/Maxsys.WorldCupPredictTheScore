using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;

public sealed class MatchPredictionsViewModel_old
{
    public MatchViewModel Match { get; set; }
    public Guid? PreviousMatchId { get; set; }
    public Guid? NextMatchId { get; set; }
    public IReadOnlyList<PredictedScoreViewModelOld> Predictions { get; set; }
}

public sealed class PredictedScoreViewModelOld
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string HomeTeamScore { get; set; }
    public string AwayTeamScore { get; set; }
    public int? Points { get; set; } = null;
}
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Prediction;

public sealed class MatchPredictionsViewModel
{
    public MatchViewModel Match { get; set; }
    public bool IsNotPlayedMatch { get; set; }
    public Guid LoggedUser { get; set; }
    public Guid? PreviousMatchId { get; set; }
    public Guid? NextMatchId { get; set; }
    public IReadOnlyList<MatchPredictionsItemViewModel> Items { get; set; }
}
public sealed class MatchPredictionsItemViewModel
{
    public UserViewModel User { get; set; }
    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }
    public int? Points { get; set; }
}
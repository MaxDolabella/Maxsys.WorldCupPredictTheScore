using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Prediction;

public sealed class UserPredictionsViewModel
{
    public UserDTO User { get; set; }
    public IReadOnlyList<UserPredictionsItemViewModel> Items { get; set; }
}

public sealed class UserPredictionsItemViewModel
{
    public MatchViewModel Match { get; set; }
    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }
    public int Points { get; set; }
}
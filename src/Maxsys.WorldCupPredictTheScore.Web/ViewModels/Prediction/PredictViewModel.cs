namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;

public sealed class PredictViewModel
{
    public Guid UserId { get; set; }
    public IList<PredictListViewModel> List { get; set; } = new List<PredictListViewModel>();

}

public sealed class PredictListViewModel
{
    public Guid MatchId { get; set; }
    public byte Round { get; set; }
    public string MatchInfo { get; set; }
    public DateTime Date { get; set; }
    public TeamViewModel HomeTeam { get; set; }
    public TeamViewModel AwayTeam { get; set; }
    public byte? HomeTeamScore { get; set; }
    public byte? AwayTeamScore { get; set; }
}
namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Results;

public sealed class ClassificationItemViewModel
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public int Rank { get; set; }
    public int Points { get; set; }
    public int? LeaderDifference { get; set; }
    public int Points25 { get; set; }
    public int Points4 { get; set; }
    public int Predictions { get; set; }
}

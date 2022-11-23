namespace Maxsys.WorldCupPredictTheScore.Web.Models.Entities;

/// <summary>
/// Palpite
/// </summary>
public sealed class PredictResult : Entity
{
    public PredictResult(Guid predictId)
        : base(Guid.NewGuid())
    {
        PredictId = predictId;
    }

    public PredictResult()
    { }

    public Guid PredictId { get; set; }
    public PredictScore Predict { get; set; }

    public int Points { get; set; }
}
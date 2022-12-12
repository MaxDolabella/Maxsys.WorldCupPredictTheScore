using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Classification;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Results;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public sealed class ClassificationService
{
    private readonly PredictionResultRepository _predictResultRepository;

    public ClassificationService(PredictionResultRepository predictResultRepository)
    {
        _predictResultRepository = predictResultRepository;
    }

    public async Task<ClassificationViewModel> List(CancellationToken cancellation = default)
    {
        var resultsByUser = (await _predictResultRepository.GetMatchResultsAsync(cancellation))
            .GroupBy(result => new { result.UserId, result.UserName });

        var classificationItems = resultsByUser
            .Select(g => new ClassificationItemViewModel
            {
                UserId = g.Key.UserId,
                UserName = g.Key.UserName[..g.Key.UserName.IndexOf('@')],
                Points = g.Sum(p => p.Points),
                Points25 = g.Where(p => p.Points == 25).Count(),
                Points4 = g.Where(p => p.Points == 4).Count(),
                Predictions = g.Count()
            })
            .OrderByDescending(item => item.Points)
            .ThenByDescending(item => item.Points25)
            .ThenByDescending(item => item.Predictions)
            .ThenBy(item => item.UserName)
            .ToList();

        var payableItems = classificationItems
            .Where(item
                => item.UserName == "flauvi.klock"
                || item.UserName == "yuri.karasawa"
                || item.UserName == "mauricio.brustolin"
                || item.UserName == "max.dolabella"
                || item.UserName == "felipe.dissenha")
            .Select(item => new ClassificationItemViewModel
            {
                UserId = item.UserId,
                UserName = item.UserName,
                Points = item.Points,
                Points25 = item.Points25,
                Points4 = item.Points4,
                Predictions = item.Predictions
            }).ToList();

        var freeRank = 1;
        var freeLeader = classificationItems.FirstOrDefault();
        classificationItems.ForEach(item =>
        {
            item.Rank = freeRank++;
            if (item.UserId != freeLeader!.UserId)
                item.LeaderDifference = item.Points - freeLeader.Points;
        });

        var payableRank = 1;
        var payableLeader = payableItems.FirstOrDefault();
        payableItems.ForEach(item =>
        {
            item.Rank = payableRank++;
            if (item.UserId != payableLeader!.UserId)
                item.LeaderDifference = item.Points - payableLeader.Points;
        });


        return new ClassificationViewModel 
        { 
            FreeItems = classificationItems, 
            PayableItems = payableItems
        };
    }
}
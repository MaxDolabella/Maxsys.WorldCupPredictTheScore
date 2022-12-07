using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;

public sealed class PredictResultRepository : RepositoryBase<PredictResult>
{
    public PredictResultRepository(ApplicationDbContext context) : base(context)
    { }

    public async Task<IReadOnlyList<PredictionPointsDTO>> GetMatchResultsAsync(Guid matchId, CancellationToken cancellation = default)
    {
        return await _dbSet
            .Where(result => result.Predict.MatchId == matchId)
            .Select(result => new PredictionPointsDTO
            {
                MatchId = result.Predict.MatchId,
                UserId = result.Predict.UserId,
                UserName = result.Predict.User.Email,
                Points = result.Points
            })
            .ToListAsync(cancellation);
    }
}
using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;

public sealed class PredictionRepository : RepositoryBase<PredictScore>
{
    public PredictionRepository(ApplicationDbContext context) : base(context)
    { }

    public async Task<IReadOnlyList<MatchPredictionsItemDTO>> GetPredictionsByMatchAsync(Guid matchId, CancellationToken cancellation = default)
    {
        return await _dbSet
            .Where(p => p.MatchId == matchId)
            .OrderBy(p => p.User.UserName)
            .SelectPredict(_context)
            .ToListAsync(cancellation);
    }
}
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

    // TODO ver PredictionResultRepository.GetUserPredictionsAsync e fazer igual.
    public async Task<IReadOnlyList<MatchPredictionsItemDTO>> GetPredictionsByMatchAsync(Guid matchId, CancellationToken cancellation = default)
    {
        return await _dbSet
            .Where(p => p.MatchId == matchId)
            .OrderBy(p => p.User.UserName)
            .SelectMatchPrediction()
            .ToListAsync(cancellation);
    }

    public async Task<MatchPredictionsDTO?> GetMatchPredictionsAsync_new(Guid matchId, CancellationToken cancellation = default)
    {
        var match = await _context.Matches
            .SelectMatch()
            .FirstOrDefaultAsync(m => m.MatchId == matchId, cancellation);

        if (match is null)
            return null;

        var items = await _dbSet
            .Where(p => p.MatchId == matchId)
            .OrderBy(p => p.User.UserName)
            .SelectMatchPrediction(_context)
            .ToListAsync(cancellation);

        return new MatchPredictionsDTO
        {
            Match = match,
            Items = items
        };
    }

    /// <summary>
    /// Obtém os Ids dos jogos já palpitados pelo usuáio.
    /// </summary>
    public async Task<IReadOnlyList<Guid>> GetPredictedMatchIdsByUserAsync(Guid userId, CancellationToken cancellation = default)
    {
        // Obter id dos jogos palpitados.
        return await _dbSet
            .Where(predicted => predicted.UserId == userId)
            .Select(predicted => predicted.MatchId)
            .ToListAsync(cancellation);
    }
}
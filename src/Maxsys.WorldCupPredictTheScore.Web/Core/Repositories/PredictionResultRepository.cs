using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;

public sealed class PredictionResultRepository : RepositoryBase<PredictResult>
{
    public PredictionResultRepository(ApplicationDbContext context) : base(context)
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

    public async Task<IReadOnlyList<PredictionPointsDTO>> GetMatchResultsAsync(CancellationToken cancellation = default)
    {
        return await _dbSet
            .Select(result => new PredictionPointsDTO
            {
                MatchId = result.Predict.MatchId,
                UserId = result.Predict.UserId,
                UserName = result.Predict.User.Email,
                Points = result.Points
            })
            .ToListAsync(cancellation);
    }

    /// <summary>
    /// Obtém os palpites de um apostador
    /// </summary>
    public async Task<UserPredictionsDTO?> GetUserPredictionsAsync(Guid userId, CancellationToken cancellation = default)
    {
        var user = await _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Email.Substring(0, u.Email.IndexOf('@'))
            })
            .FirstOrDefaultAsync(u => u.Id == userId, cancellation);

        if (user is null)
            return null;

        var items = await _dbSet
            .Where(predictionResult => predictionResult.Predict.UserId == userId)
            .OrderBy(predictionResult => predictionResult.Predict.Match.Date)
            .SelectUserPredictionsItem()
            .ToListAsync(cancellation);

        return new UserPredictionsDTO
        {
            User = user,
            Items = items
        };
    }
}
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public class ResultPointsService
{
    private readonly ApplicationDbContext _context;

    public ResultPointsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PredictPointsDTO>> GetPointsByMatchAsync(Guid matchId, CancellationToken cancellation = default)
    {
        return await _context.Results.AsNoTracking()
            .Where(result => result.Predict.MatchId == matchId)
            .Select(result => new PredictPointsDTO
            {
                PredictId = result.PredictId,
                MatchId = result.Predict.MatchId,
                UserId = result.Predict.UserId,
                Points = result.Points
            })
            .ToListAsync(cancellation);
    }

    private static int GetPoints(MatchResultDTO match, MatchResultDTO prediction)
    {
        // Acertar o placar exato da partida
        if (prediction.HomeScore == match.HomeScore && prediction.AwayScore == match.AwayScore)
            return 25;

        // Acertar o vencedor e o número de gols da equipe vencedora
        if (prediction.Winner == match.Winner && prediction.WinnerScore == match.WinnerScore)
            return 18;

        // Acertar o vencedor e a diferença de gols entre o vencedor e o perdedor
        // Acertar que a partida terminaria em empate
        if (prediction.Winner == match.Winner && prediction.ScoreDifference == match.ScoreDifference)
            return 15;

        // Acertar o time vencedor e o número de gols do time perdedor
        if (prediction.Winner == match.Winner && prediction.LoserScore == match.LoserScore)
            return 12;

        // Acertar apenas o vencedor da partida
        if (prediction.Winner == match.Winner)
            return 10;

        return 4; // Consolação
    }
}
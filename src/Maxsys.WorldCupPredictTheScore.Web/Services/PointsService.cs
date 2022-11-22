using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public class PointsService
{
    private readonly ApplicationDbContext _context;

    public PointsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdatePointsAsync(Guid matchId, CancellationToken cancellation = default)
    {
        // apagar as pontuações (palpites-resultado) referente à esse jogo (caso existam)
        var currentResultId = await _context.Results.Where(p => p.Predict.MatchId == matchId).Select(e => e.Id).ToListAsync(cancellation);
        foreach (var id in currentResultId)
        {
            var entity = new PredictResult() { Id = id };
            _context.Results.Attach(entity);
            _context.Results.Remove(entity);
        }

        // Obtém o resultado de fato
        var match = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(m => m.Id == matchId, cancellation);
        if (match is null)
            throw new NullReferenceException();

        // Obtém os palpites para essa partida
        var predictionResults = await _context.Predictions.AsNoTracking()
            .Where(p => p.MatchId == match.Id)
            .ToListAsync(cancellation);

        // cria e atribui a (nova) pontuação para todos os palpites.
        var matchResult = new MatchResultDTO(match.HomeScore!.Value, match.AwayScore!.Value);
        var results = new List<PredictResult>();
        foreach (var prediction in predictionResults)
        {
            var result = new PredictResult(prediction.Id)
            {
                Points = GetPoints(matchResult, new MatchResultDTO(prediction.HomeTeamScore, prediction.AwayTeamScore))
            };

            results.Add(result);
        }

        await _context.AddRangeAsync(results, cancellation);
        await _context.SaveChangesAsync(cancellation);
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


public class ResultsService
{
    private readonly ApplicationDbContext _context;

    public ResultsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task GetResultsByMatchAsync(Guid matchId, CancellationToken cancellation = default)
    {
        var models = await _context.Results.AsNoTracking()
            .Select(pr => new { pr.Predict.User.Email, pr.Points })
            .GroupBy(a => a.Email)
            .Select(g => new UserPointsViewModel
            {
                UserName = g.Key,
                Points = g.Sum(a => a.Points)
            })
            .OrderByDescending(v => v.Points)
            .ToListAsync(cancellation);

        // apagar as pontuações (palpites-resultado) referente à esse jogo (caso existam)
        var currentResultId = await _context.Results.Where(p => p.Predict.MatchId == matchId).Select(e => e.Id).ToListAsync(cancellation);
        foreach (var id in currentResultId)
        {
            var entity = new PredictResult() { Id = id };
            _context.Results.Attach(entity);
            _context.Results.Remove(entity);
        }

        // Obtém o resultado de fato
        var match = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(m => m.Id == matchId, cancellation);
        if (match is null)
            throw new NullReferenceException();

        // Obtém os palpites para essa partida
        var predictionResults = await _context.Predictions.AsNoTracking()
            .Where(p => p.MatchId == match.Id)
            .ToListAsync(cancellation);

        // cria e atribui a (nova) pontuação para todos os palpites.
        var matchResult = new MatchResultDTO(match.HomeScore!.Value, match.AwayScore!.Value);
        var results = new List<PredictResult>();
        foreach (var prediction in predictionResults)
        {
            var result = new PredictResult(prediction.Id)
            {
                Points = GetPoints(matchResult, new MatchResultDTO(prediction.HomeTeamScore, prediction.AwayTeamScore))
            };

            results.Add(result);
        }

        await _context.AddRangeAsync(results, cancellation);
        await _context.SaveChangesAsync(cancellation);
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
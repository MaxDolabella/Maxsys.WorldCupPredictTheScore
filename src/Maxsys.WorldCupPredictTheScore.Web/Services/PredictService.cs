using FluentValidation.Results;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public sealed class PredictService
{
    private readonly ApplicationDbContext _context;

    public PredictService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém o próximo jogo de cada time.
    /// </summary>
    public async Task<IEnumerable<PredictListDTO>> GetUserAvailableMatchesToPredictAsync(Guid userId, CancellationToken cancellation = default)
    {
        // Obter data (agora)
        var now = DateTime.Now;

        // Obter id dos jogos palpitados.
        var predictedMatchIds = await _context.Predictions.AsNoTracking()
            .Where(predicted => predicted.UserId == userId)
            .Select(predicted => predicted.MatchId)
            .ToListAsync(cancellation);

        var allmatches = await _context.Matches.AsNoTracking()
            .Where(m => m.HomeScore == null && m.AwayScore == null) // jogos ainda não jogados
            .OrderBy(m => m.Date)
            .SelectMatch()
            .ToListAsync(cancellation);

        // próximo jogo de cada seleção
        var nextMatches = new List<MatchDTO>();
        foreach (var match in allmatches)
        {
            if (!nextMatches.Any(nm
                => nm.HomeTeam.Id == match.HomeTeam.Id || nm.AwayTeam.Id == match.HomeTeam.Id
                || nm.HomeTeam.Id == match.AwayTeam.Id || nm.AwayTeam.Id == match.AwayTeam.Id))
            {
                nextMatches.Add(match);
            }
        }

        var nextMatchesToPredict = nextMatches
            .Where(m => m.Date > now) // jogos não iniciados
            .Where(m => !predictedMatchIds.Contains(m.MatchId)) // Jogos ainda não palpitados
            .Select(m => new PredictListDTO
            {
                MatchId = m.MatchId,
                MatchInfo = m.Round switch
                {
                    4 => "Oitavas de final",
                    5 => "Quartas de final",
                    6 => "Semifinal",
                    7 => "Decisão de 3º Lugar",
                    8 => "FINAL",
                    _ => $"Rodada {m.Round} / Grupo {m.Group}"
                },
                Date = m.Date,
                HomeTeam = m.HomeTeam,
                AwayTeam = m.AwayTeam
            })
            .OrderBy(m => m.Date)
            .ToList();

        return nextMatchesToPredict;
    }

    public async Task<ValidationResult> SaveUserPredictsAsync(IEnumerable<PredictScore> predicts, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        foreach (var predict in predicts)
        {
            var exists = await _context.Predictions.AsNoTracking().AnyAsync(p => p.UserId == predict.Id && p.MatchId == predict.MatchId, cancellation);
            if (exists)
                return validationResult.AddError($"Já existe um palpite deste usuário para o jogo {predict.MatchId}");

            var entry = await _context.Predictions.AddAsync(predict, cancellation);
            if (entry.State != EntityState.Added)
                return validationResult.AddError($"Falha ao adicionar palpite para o jogo {predict.MatchId}");
        }

        try
        {
            _ = await _context.SaveChangesAsync(cancellation);
        }
        catch (Exception ex)
        {
            _ = validationResult.AddError($"Falha ao adicionar palpite para o jogo: {ex}");
        }

        return validationResult;
    }

    public async Task<MatchPredictionsDTO?> GetMatchPredictionsAsync(Guid matchId, Guid userId, DateTime date, CancellationToken cancellation = default)
    {
        var match = await _context.Matches.AsNoTracking()
            .SelectMatch()
            .FirstOrDefaultAsync(m => m.MatchId == matchId, cancellation);

        // TODO exception aqui.
        // Por enquanto retorna lista vazia
        if (match is null)
            return null;

        var query = _context.Predictions.AsNoTracking()
            .Where(p => p.MatchId == matchId);

        // não visualizar palpites de adversários onde a partida não começou
        if (match.Date > date)
            query = query.Where(p => p.UserId == userId);

        var predictions = await query
            .OrderBy(p => p.User.UserName)
            .Select(p => new PredictedScoreDTO
            {
                PredictionId = p.Id,
                UserName = p.User.UserName,
                HomeTeamScore = p.HomeTeamScore,
                AwayTeamScore = p.AwayTeamScore
            })
            .ToListAsync(cancellation);

        return new MatchPredictionsDTO
        {
            Match = match,
            Predictions = predictions
        };
    }
}
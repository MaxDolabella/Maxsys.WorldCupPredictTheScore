using FluentValidation.Results;
using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public sealed class MatchService
{
    private readonly ApplicationDbContext _context;

    public MatchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MatchDTO>> GetAllMatchesAsync(CancellationToken cancellation = default)
    {
        return await _context.Matches.AsNoTracking()
            .OrderBy(m => m.Date)
            .SelectMatch()
            .ToListAsync(cancellation);
    }

    public async Task<MatchDTO?> GetMatchAsync(Guid matchId, CancellationToken cancellation = default)
    {
        return await _context.Matches.AsNoTracking()
            .SelectMatch()
            .FirstOrDefaultAsync(m => m.MatchId == matchId, cancellation);
    }

    public async ValueTask<ValidationResult> UpdateMatchAsync(MatchEditDTO model, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        if ((model.HomeTeamScore is null && model.AwayTeamScore is not null)
            || (model.AwayTeamScore is null && model.HomeTeamScore is not null))
            return validationResult.AddError("Placar de um dos times está faltando.");

        // atualizar o placar
        var match = await _context.Matches.FindAsync(new object?[] { model.MatchId }, cancellation);
        if (match is null)
            return validationResult.AddError("Jogo não encontrado durante update.");

        match.HomeScore = model.HomeTeamScore;
        match.AwayScore = model.AwayTeamScore;

        return await CommitAsync("Ocorreu um erro ao atualizar o jogo.", cancellation);
    }

    public async ValueTask<ValidationResult> AddMatchAsync(MatchCreateDTO model, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        // DTO to Entity
        var entity = new Match(model.Group, model.Round, model.Date, model.HomeTeamId, model.AwayTeamId);

        // Validation Here ??

        if ((await _context.Matches.AddAsync(entity, cancellation)).State != EntityState.Added)
            return validationResult.AddError("Ocorreu um erro ao adicionar o jogo.");

        return await CommitAsync("Ocorreu um erro ao salvar novo jogo.", cancellation);
    }

    private async ValueTask<ValidationResult> CommitAsync(string errorMessage, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();
        try
        {
            await _context.SaveChangesAsync(cancellation);
        }
        catch (Exception ex)
        {
            validationResult.AddError($"{errorMessage}: {ex}");
        }

        return validationResult;
    }
}
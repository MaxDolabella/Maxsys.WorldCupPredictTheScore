using FluentValidation.Results;
using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;

public sealed class MatchRepository : RepositoryBase<Match>
{
    public MatchRepository(ApplicationDbContext context) : base(context)
    { }

    public async Task<IReadOnlyList<MatchDTO>> GetAllAsync(CancellationToken cancellation = default)
    {
        return await _dbSet
            .OrderBy(m => m.Date)
            .SelectMatch()
            .ToListAsync(cancellation);
    }

    public async Task<MatchDTO?> GetByIdAsync(Guid matchId, CancellationToken cancellation = default)
    {
        return await _dbSet
            .SelectMatch()
            .FirstOrDefaultAsync(m => m.MatchId == matchId, cancellation);
    }

    public async ValueTask<ValidationResult> UpdateAsync(MatchEditDTO model, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        // mover para validation
        if ((model.HomeTeamScore is null && model.AwayTeamScore is not null)
            || (model.AwayTeamScore is null && model.HomeTeamScore is not null))
            return validationResult.AddError("Placar de um dos times está faltando.");

        // atualizar o placar
        var match = await _dbSet.FindAsync(new object?[] { model.MatchId }, cancellation);
        if (match is null)
            return validationResult.AddError("Jogo não encontrado durante update.");

        match.HomeScore = model.HomeTeamScore;
        match.AwayScore = model.AwayTeamScore;

        return await CommitAsync("Ocorreu um erro ao atualizar o jogo.", cancellation);
    }

    public async ValueTask<ValidationResult> AddAsync(MatchCreateDTO model, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        // DTO to Entity
        var entity = new Match(model.Group, model.Round, model.Date, model.HomeTeamId, model.AwayTeamId);

        // Validation Here ??

        if ((await _dbSet.AddAsync(entity, cancellation)).State != EntityState.Added)
            return validationResult.AddError("Ocorreu um erro ao adicionar o jogo.");

        return await CommitAsync("Ocorreu um erro ao salvar novo jogo.", cancellation);
    }

    public async Task<IList<Guid>> GetMatchesIdsAsync(CancellationToken cancellation = default)
    {
        return await _dbSet
            .OrderBy(match => match.Date)
            .Select(match => match.Id)
            .ToListAsync(cancellation);
    }
}
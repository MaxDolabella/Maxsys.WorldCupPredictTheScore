using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;

public sealed class TeamRepository : RepositoryBase<Team>
{
    public TeamRepository(ApplicationDbContext context) : base(context)
    { }

    public async Task<IReadOnlyList<TeamDTO>> GetAllAsync(CancellationToken cancellation = default)
    {
        return await _dbSet
            .OrderBy(m => m.Name)
            .SelectTeam()
            .ToListAsync(cancellation);
    }

    public async Task<TeamDTO?> GetByIdAsync(Guid teamId, CancellationToken cancellation = default)
    {
        return await _dbSet
            .SelectTeam()
            .FirstOrDefaultAsync(m => m.Id == teamId, cancellation);
    }

    /*
    public async ValueTask<ValidationResult> UpdateAsync(TeamEditDTO model, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        // Validation aqui

        // atualizar o placar
        var team = await _dbSet.FindAsync(new object?[] { model.MatchId }, cancellation);
        if (team is null)
            return validationResult.AddError("Time não encontrado durante update.");

        team.Update(model);

        return await CommitAsync("Ocorreu um erro ao atualizar o time.", cancellation);
    }

    public async ValueTask<ValidationResult> AddAsync(TeamCreateDTO model, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        // DTO to Entity
        var entity = new Team(model.Name, model.Code);

        // Validation Here

        if ((await _dbSet.AddAsync(entity, cancellation)).State != EntityState.Added)
            return validationResult.AddError("Ocorreu um erro ao adicionar o time.");

        return await CommitAsync("Ocorreu um erro ao salvar novo time.", cancellation);
    }

    */
}
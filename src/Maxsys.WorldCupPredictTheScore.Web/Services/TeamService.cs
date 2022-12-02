using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public sealed class TeamService
{
    private readonly ApplicationDbContext _context;

    public TeamService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TeamDTO>> GetAllTeamsAsync(CancellationToken cancellation = default)
    {
        return await _context.Teams.AsNoTracking()
            .OrderBy(m => m.Name)
            .SelectTeam()
            .ToListAsync(cancellation);
    }
}
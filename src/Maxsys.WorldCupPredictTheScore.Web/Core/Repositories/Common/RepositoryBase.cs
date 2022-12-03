using Maxsys.WorldCupPredictTheScore.Web.Data;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;

public abstract class RepositoryBase
{
    protected readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }
}
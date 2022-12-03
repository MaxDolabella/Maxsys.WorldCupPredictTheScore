using Maxsys.WorldCupPredictTheScore.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;

public abstract class RepositoryBase<T> : RepositoryBase where T : class
{
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(ApplicationDbContext context)
        : base(context)
    {
        _dbSet = context.Set<T>();
    }
}
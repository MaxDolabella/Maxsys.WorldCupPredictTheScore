using FluentValidation.Results;
using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Data;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Repositories.Common;

public abstract class RepositoryBase
{
    protected readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }

    protected async ValueTask<ValidationResult> CommitAsync(string errorMessage, CancellationToken cancellation = default)
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
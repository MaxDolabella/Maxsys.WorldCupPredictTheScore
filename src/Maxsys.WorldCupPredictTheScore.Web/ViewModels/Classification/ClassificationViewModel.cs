using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Results;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Classification;

public sealed class ClassificationViewModel
{
    public Guid? LoggedUserId { get; set; }
    public IReadOnlyList<ClassificationItemViewModel> FreeItems { get; set; } = new List<ClassificationItemViewModel>();
    public IReadOnlyList<ClassificationItemViewModel> PayableItems { get; set; } = new List<ClassificationItemViewModel>();
}
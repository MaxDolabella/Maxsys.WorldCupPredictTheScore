using Maxsys.WorldCupPredictTheScore.Web.Controllers.Base;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[AllowAnonymous]
[Route("resultados")]
public class ClassificationController : BaseController
{
    private readonly ClassificationService _classificationService;

    public ClassificationController(ClassificationService classificationService)
    {
        _classificationService = classificationService;
    }

    [Route("ranking")]
    public async Task<IActionResult> Index(CancellationToken cancellation = default)
    {
        var viewModels = await _classificationService.List(cancellation);
        var loggedUser = TryGetLoggedUser(acceptNullResult: true);

        viewModels.LoggedUserId = loggedUser?.Id;

        return View(viewModels);
    }

}
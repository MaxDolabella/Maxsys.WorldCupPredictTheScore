using AutoMapper;
using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Maxsys.WorldCupPredictTheScore.Web.Controllers.Base;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[Authorize(Roles = "admin,user")]
[Route("palpites")]
public class PredictController : BaseController
{
    private readonly IMapper _mapper;
    private readonly PredictionService _predictService;
    private readonly ResultPointsService _resultPointsService;
    private readonly UserManager<AppUser> _userManager;

    public PredictController(PredictionService predictService, ResultPointsService resultPointsService, UserManager<AppUser> userManager, IMapper mapper)
    {
        _predictService = predictService;
        _userManager = userManager;
        _resultPointsService = resultPointsService;
        _mapper = mapper;
    }

    [Authorize(Roles = "user")]
    public async Task<IActionResult> Index(CancellationToken cancellation = default)
    {
        var viewModel = await _predictService.GetUserAvailableMatchesToPredictAsync(LoggedUser.Id, cancellation);

        return View(viewModel);
    }

    [Authorize(Roles = "user")]
    [HttpPost]
    public async Task<IActionResult> Index(PredictViewModel viewModel, CancellationToken cancellation = default)
    {
        var models = GetModelsFromViewModel(viewModel);

        var result = await _predictService.SaveUserPredictsAsync(models, cancellation);

        return result.IsValid ? RedirectToAction(nameof(Index)) : BadRequest(result.Errors);
    }

    [HttpGet]
    [Route("partida/{matchId:guid}")]
    public async Task<IActionResult> MatchPredictions(Guid matchId, CancellationToken cancellation = default)
    {
        var loggerUser = LoggedUser;
        if (loggerUser is null)
            throw new ApplicationException("Falha ao ler usuário logado.");

        var viewModel = await _predictService.MatchPredictionsAsync(matchId, loggerUser.Id, cancellation);

        return viewModel is not null ? View(viewModel) : NotFound();
    }

    private IList<PredictScore> GetModelsFromViewModel(PredictViewModel viewModel)
    {
        var userId = LoggedUser.Id;
        var now = DateTime.UtcNow;

        return viewModel.List
            .Where(vm => vm.Date >= now)
            .Where(vm => vm.HomeTeamScore is not null && vm.AwayTeamScore is not null)
            .Select(vm => new PredictScore(vm.MatchId, userId, vm.HomeTeamScore.Value, vm.AwayTeamScore.Value))
            .ToList();
    }
}
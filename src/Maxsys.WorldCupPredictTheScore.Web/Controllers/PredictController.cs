using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[Authorize(Roles = "user")]
[Route("palpites")]
public class PredictController : Controller
{
    private readonly PredictService _predictService;
    private readonly UserManager<AppUser> _userManager;

    public PredictController(PredictService predictService, UserManager<AppUser> userManager)
    {
        _predictService = predictService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(CancellationToken cancellation = default)
    {
        var userId = GetLoggedUserId();

        var availableMatchesToPredict = await _predictService.GetUserAvailableMatchesToPredictAsync(userId, cancellation);

        var viewModel = new PredictViewModel
        {
            UserId = userId,
            List = availableMatchesToPredict.Select(predictMatch => new PredictListViewModel
            {
                MatchId = predictMatch.MatchId,
                MatchInfo = predictMatch.MatchInfo,
                Date = predictMatch.Date,
                HomeTeam = new TeamViewModel
                {
                    Name = predictMatch.HomeTeam.Name,
                    Code = predictMatch.HomeTeam.Code,
                    Flag = $"{predictMatch.HomeTeam.Code}.webp"
                },
                AwayTeam = new TeamViewModel
                {
                    Name = predictMatch.AwayTeam.Name,
                    Code = predictMatch.AwayTeam.Code,
                    Flag = $"{predictMatch.AwayTeam.Code}.webp"
                },
                HomeTeamScore = null,
                AwayTeamScore = null
            }).ToList()
        };

        return View(viewModel);
    }

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
        var userId = GetLoggedUserId();
        var dateNow = DateTime.Now;

        var model = await _predictService.GetMatchPredictionsAsync(matchId, userId, dateNow, cancellation);
        if (model is null)
            return NotFound();

        var viewModel = new MatchPredictionsViewModel
        {
            Match = new MatchViewModel
            {
                MatchId = model.Match.MatchId,
                MatchInfo = model.Match.Round switch
                {
                    4 => "Oitavas de final",
                    5 => "Quartas de final",
                    6 => "Semifinal",
                    7 => "Decisão de 3º Lugar",
                    8 => "FINAL",
                    _ => $"Rodada {model.Match.Round} / Grupo {model.Match.Group}"
                },
                Date = model.Match.Date,
                HomeTeam = new TeamViewModel
                {
                    Name = model.Match.HomeTeam.Name,
                    Code = model.Match.HomeTeam.Code,
                    Flag = $"{model.Match.HomeTeam.Code}.webp",
                },
                AwayTeam = new TeamViewModel
                {
                    Name = model.Match.AwayTeam.Name,
                    Code = model.Match.AwayTeam.Code,
                    Flag = $"{model.Match.AwayTeam.Code}.webp",
                },
                HomeTeamScore = model.Match.HomeTeamScore,
                AwayTeamScore = model.Match.AwayTeamScore
            },
            Predictions = model.Predictions
                .Select(p => new PredictedScoreViewModel
                {
                    UserName = p.UserName.Substring(0, p.UserName.IndexOf('@')),
                    HomeTeamScore = p.HomeTeamScore,
                    AwayTeamScore = p.AwayTeamScore
                })
                .ToList()
        };

        return View(viewModel);
    }

    private Guid GetLoggedUserId()
    {
        var currentUserID = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrWhiteSpace(currentUserID)
            ? Guid.Parse(currentUserID)
            : throw new ArgumentNullException();
    }

    private IList<PredictScore> GetModelsFromViewModel(PredictViewModel viewModel)
    {
        var userId = GetLoggedUserId();
        var now = DateTime.Now;

        return viewModel.List
            .Where(vm => vm.Date >= now)
            .Where(vm => vm.HomeTeamScore is not null && vm.AwayTeamScore is not null)
            .Select(vm => new PredictScore(vm.MatchId, userId, vm.HomeTeamScore.Value, vm.AwayTeamScore.Value))
            .ToList();
    }
}
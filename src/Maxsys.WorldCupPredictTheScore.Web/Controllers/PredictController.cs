using System.Security.Claims;
using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[Authorize(Roles = "admin,user")]
[Route("palpites")]
public class PredictController : Controller
{
    private readonly PredictionService _predictService;
    private readonly ResultPointsService _resultPointsService;
    private readonly UserManager<AppUser> _userManager;

    public PredictController(PredictionService predictService, ResultPointsService resultPointsService, UserManager<AppUser> userManager)
    {
        _predictService = predictService;
        _userManager = userManager;
        _resultPointsService = resultPointsService;
    }

    [Authorize(Roles = "user")]
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
        var userId = GetLoggedUserId();
        var dateNow = DateTime.UtcNow;

        var model = await _predictService.GetMatchPredictionsAsync(matchId, cancellation);
        if (model is null)
            return NotFound();

        var matchesIds = await _predictService.GetMatchesIdsAsync(cancellation);
        var currentIndex = matchesIds.ToList().IndexOf(matchId);
        var previousMatchId = currentIndex > 0 ? matchesIds[currentIndex - 1] : default(Guid?);
        var nextMatchId = currentIndex < matchesIds.Count - 1 ? matchesIds[currentIndex + 1] : default(Guid?);

        var match = model.Match;
        var notPlayedMatch = match.Date > dateNow; // não visualizar palpites de adversários onde a partida não começou
        var viewModel = new MatchPredictionsViewModel
        {
            NextMatchId = nextMatchId,
            PreviousMatchId = previousMatchId,
            Match = new MatchViewModel
            {
                MatchId = match.MatchId,
                MatchInfo = match.Round switch
                {
                    4 => "Oitavas de final",
                    5 => "Quartas de final",
                    6 => "Semifinal",
                    7 => "Decisão de 3º Lugar",
                    8 => "FINAL",
                    _ => $"Rodada {match.Round} / Grupo {match.Group}"
                },
                Date = match.Date,
                HomeTeam = new TeamViewModel
                {
                    Name = match.HomeTeam.Name,
                    Code = match.HomeTeam.Code,
                    Flag = $"{match.HomeTeam.Code}.webp",
                },
                AwayTeam = new TeamViewModel
                {
                    Name = match.AwayTeam.Name,
                    Code = match.AwayTeam.Code,
                    Flag = $"{match.AwayTeam.Code}.webp",
                },
                HomeTeamScore = match.HomeTeamScore,
                AwayTeamScore = match.AwayTeamScore
            },
            Predictions = model.Predictions
                .Select(predictedScore => new PredictedScoreViewModel
                {
                    UserId = predictedScore.UserId,
                    UserName = predictedScore.UserName[..predictedScore.UserName.IndexOf('@')],
                    HomeTeamScore = notPlayedMatch && predictedScore.UserId != userId ? "?" : predictedScore.HomeTeamScore.ToString(),
                    AwayTeamScore = notPlayedMatch && predictedScore.UserId != userId ? "?" : predictedScore.AwayTeamScore.ToString()
                })
                .ToList()
        };

        // Obter pontos da partida
        var matchResults = await _resultPointsService.GetPointsByMatchAsync(matchId, cancellation);
        // caso tenha pontos (partida em andamento ou finalizada) exibir
        foreach (var predictionScore in viewModel.Predictions)
        {
            var resultPoints = matchResults.SingleOrDefault(points => points.UserId == predictionScore.UserId);
            if (resultPoints is not null)
                predictionScore.Points = resultPoints.Points;
        }

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
        var now = DateTime.UtcNow;

        return viewModel.List
            .Where(vm => vm.Date >= now)
            .Where(vm => vm.HomeTeamScore is not null && vm.AwayTeamScore is not null)
            .Select(vm => new PredictScore(vm.MatchId, userId, vm.HomeTeamScore.Value, vm.AwayTeamScore.Value))
            .ToList();
    }
}
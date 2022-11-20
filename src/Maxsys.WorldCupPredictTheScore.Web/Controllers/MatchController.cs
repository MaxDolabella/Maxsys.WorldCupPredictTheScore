using FluentValidation.AspNetCore;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[Authorize(Roles = "admin, user")]
[Route("jogos")]
public class MatchController : Controller
{
    private readonly ILogger _logger;
    private readonly MatchService _matchService;
    private readonly PointsService _pointsService;

    public MatchController(ILogger<MatchController> logger, MatchService matchService, PointsService pointsService)
    {
        _logger = logger;
        _matchService = matchService;
        _pointsService = pointsService;
    }


    public async Task<IActionResult> Index(CancellationToken cancellation = default)
    {
        var models = await _matchService.GetAllMatchesAsync(cancellation);

        var viewModels = models
            .Select(match => new MatchViewModel
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
            AwayTeamScore = match.AwayTeamScore,
            //IsPlayed = (match.HomeTeamScore, match.AwayTeamScore) is not (null, null)
        })
            .ToList();

        return View(viewModels);
    }


    [Authorize(Roles = "admin")]
    [Route("editar/{matchId:guid}")]
    public async Task<IActionResult> Edit(Guid matchId, CancellationToken cancellation = default)
    {
        var model = await _matchService.GetMatchAsync(matchId, cancellation);

        if (model is null)
            return NotFound();

        var viewModel = new MatchEditViewModel
        {
            MatchId = model.MatchId,
            HomeTeamName = model.HomeTeam.Name,
            AwayTeamName = model.AwayTeam.Name,
            HomeTeamScore = model.HomeTeamScore,
            AwayTeamScore = model.AwayTeamScore
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [Route("editar/{matchId:guid}")]
    public async ValueTask<IActionResult> Edit(MatchEditViewModel viewModel, CancellationToken cancellation = default)
    {
        var model = new MatchEditDTO
        {
            MatchId = viewModel.MatchId,
            HomeTeamScore = viewModel.HomeTeamScore,
            AwayTeamScore = viewModel.AwayTeamScore
        };

        var result = await _matchService.UpdateMatchAsync(model, cancellation);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState, null);
            return View(viewModel);
        }

        // points
        await _pointsService.UpdatePointsAsync(model.MatchId, cancellation);

        return RedirectToAction("Index");
    }
}
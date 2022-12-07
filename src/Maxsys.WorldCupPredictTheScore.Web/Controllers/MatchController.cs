using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly MatchService _matchService;
    private readonly TeamService _teamService;
    private readonly PointsService _pointsService;

    public MatchController(IMapper mapper, MatchService matchService, TeamService teamService, PointsService pointsService)
    {
        _mapper = mapper;
        _matchService = matchService;
        _teamService = teamService;
        _pointsService = pointsService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellation = default)
    {
        var models = await _matchService.GetAllMatchesAsync(cancellation);

        var viewModels = _mapper.Map<IEnumerable<MatchViewModel>>(models);
        return View(viewModels.ToList());
    }

    [Authorize(Roles = "admin")]
    [Route("editar/{matchId:guid}")]
    public async Task<IActionResult> Edit(Guid matchId, CancellationToken cancellation = default)
    {
        var model = await _matchService.GetMatchAsync(matchId, cancellation);
        if (model is null)
            return NotFound();

        return View(_mapper.Map<MatchEditViewModel>(model));
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [Route("editar/{matchId:guid}")]
    public async ValueTask<IActionResult> Edit(MatchEditViewModel viewModel, CancellationToken cancellation = default)
    {
        var model = _mapper.Map<MatchEditDTO>(viewModel);

        var result = await _matchService.UpdateMatchAsync(model, cancellation);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState, null);
            return View(viewModel);
        }

        // points
        await _pointsService.UpdatePointsAsync(model.MatchId, cancellation);

        return RedirectToAction("Edit", model.MatchId);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("create")]
    public async ValueTask<IActionResult> Create(CancellationToken cancellation = default)
    {
        var teams = await GetTeamViewModelsAsync(cancellation);

        var viewModel = new MatchCreateViewModel
        {
            Teams = teams,

            MatchDate = null,
            SelectedHomeTeamId = null,
            SelectedAwayTeamId = null
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [Route("create")]
    public async ValueTask<IActionResult> Create(MatchCreateViewModel viewModel, CancellationToken cancellation = default)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Teams = await GetTeamViewModelsAsync(cancellation);
            return View(viewModel);
        }

        var model = new MatchCreateDTO
        {
            Date = viewModel.MatchDate!.Value,
            Group = viewModel.Group ?? ' ',
            Round = viewModel.SelectedRound!.Value,
            HomeTeamId = viewModel.SelectedHomeTeamId!.Value,
            AwayTeamId = viewModel.SelectedAwayTeamId!.Value
        };

        var result = await _matchService.AddMatchAsync(model, cancellation);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState, null);
            viewModel.Teams = await GetTeamViewModelsAsync(cancellation);
            return View(viewModel);
        }

        return RedirectToAction("Index");
    }

    private async Task<IReadOnlyList<TeamViewModel>> GetTeamViewModelsAsync(CancellationToken cancellation)
    {
        var teams = await _teamService.GetAllTeamsAsync(cancellation);

        return teams.Select(team => new TeamViewModel
        {
            Id = team.Id,
            Name = team.Name,
            Code = team.Code,
            Flag = $"{team.Code}.webp",
        }).ToList();
    }
}
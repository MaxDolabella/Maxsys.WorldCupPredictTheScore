using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[Authorize(Roles = "admin")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly PredictService _predictService;

    public AdminController(ApplicationDbContext context, PredictService predictService)
    {
        _context = context;
        _predictService = predictService;
    }

    [HttpGet]
    [Route("adicionar-palpite-retroativo")]
    public async Task<IActionResult> RetroativeRegisterPredict()
    {
        var users = await _context.Users.Select(u => new UserSelectViewModel
        {
            UserId = u.Id,
            UserEmail = u.Email
        })
        .ToListAsync();

        var matches = await _context.Matches.Select(m => new MatchSelectViewModel
        {
            MatchId = m.Id,
            MatchDescription = $"{m.HomeTeam.Name} X {m.AwayTeam.Name}"
        })
        .ToListAsync();

        var viewModel = new PrecitcExtraCreateViewModel
        {
            UsersSelect = users,
            MatchesSelect = matches,
            SelectedMatchId = null,
            SelectedUserId = null,
            HomeTeamScore = null,
            AwayTeamScore = null
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("adicionar-palpite-retroativo")]
    public async ValueTask<IActionResult> RetroativeRegisterPredict(PrecitcExtraCreateViewModel viewModel, CancellationToken cancellation = default)
    {
        if (viewModel.SelectedUserId is null) ModelState.AddModelError("SelectedUserId", "Selecione um usuário.");
        if (viewModel.SelectedMatchId is null) ModelState.AddModelError("SelectedMatchId", "Selecione uma partida.");
        if (viewModel.HomeTeamScore is null) ModelState.AddModelError("HomeTeamScore", "Placar obrigatório.");
        if (viewModel.AwayTeamScore is null) ModelState.AddModelError("AwayTeamScore", "Placar obrigatório.");

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "É necessario recarregar a página.");
            return View(viewModel);
        }

        var predict = new PredictScore(viewModel.SelectedMatchId.Value, viewModel.SelectedUserId.Value, viewModel.HomeTeamScore.Value, viewModel.AwayTeamScore.Value);

        var result = await _predictService.SaveUserPredictsAsync(new PredictScore[] { predict }, cancellation);

        return result.IsValid ? RedirectToAction(nameof(RetroativeRegisterPredict)) : BadRequest(result.Errors);
    }


    //[Route("atualizar-datas-utc")]
    //public async ValueTask<IActionResult> UTCDateCorrection()
    //{
        
    //    var matches = await _context.Matches.AsTracking().ToListAsync();

    //    try
    //    {
    //        foreach (var item in matches)
    //        {
    //            item.Date = item.Date.AddHours(3);
    //        }

    //        await _context.SaveChangesAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }

    //    return Ok("Datas UTC Atualizadas.");
    //}

}

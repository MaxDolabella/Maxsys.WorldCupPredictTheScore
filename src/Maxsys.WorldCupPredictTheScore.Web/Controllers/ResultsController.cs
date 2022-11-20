using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[AllowAnonymous]
[Route("resultados")]
public class ResultsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ResultsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Route("ranking")]
    public async Task<IActionResult> Index(CancellationToken cancellation = default)
    {
        var viewModels = await _context.Results.AsNoTracking()
            .Select(pr => new { pr.Predict.User.Email, pr.Points })
            .GroupBy(a => a.Email)
            .Select(g => new UserPointsViewModel
            {
                UserName = g.Key,
                Points = g.Sum(a => a.Points)
            })
            .OrderByDescending(v => v.Points)
            .ToListAsync(cancellation);

        foreach (var viewModel in viewModels)
        {
            viewModel.UserName = viewModel.UserName[..viewModel.UserName.IndexOf('@')];
        }

        return View(viewModels);
    }
}
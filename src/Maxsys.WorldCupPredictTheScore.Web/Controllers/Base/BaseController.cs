using Maxsys.WorldCupPredictTheScore.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers.Base;

public abstract class BaseController : Controller
{
    protected LoggedUserViewModel LoggedUser => TryGetLoggedUser(acceptNullResult: false)!;

    protected LoggedUserViewModel? TryGetLoggedUser(bool acceptNullResult = true)
    {
        var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserID))
            return acceptNullResult ? null : throw new Exception("Usuário não logado");

        var name = User.FindFirst(ClaimTypes.Email)?.Value ?? "Not found user email";
        var roles = User.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToList();

        return new LoggedUserViewModel
        {
            Id = Guid.Parse(currentUserID),
            Name = name,
            Roles = roles
        };
    }
}
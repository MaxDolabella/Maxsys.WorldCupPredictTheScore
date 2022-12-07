using System.Security.Claims;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers.Base;

public abstract class BaseController : Controller
{
    //private readonly UserManager<AppUser> _userManager;
    protected LoggedUserViewModel LoggedUser => GetLoggedUser();

    private LoggedUserViewModel GetLoggedUser()
    {
        var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserID))
            throw new Exception("Usuário não logado");

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
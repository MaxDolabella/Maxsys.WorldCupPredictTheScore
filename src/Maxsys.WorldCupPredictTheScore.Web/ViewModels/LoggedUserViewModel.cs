using Maxsys.WorldCupPredictTheScore.Web.Models;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels;

public sealed class LoggedUserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> Roles { get; set; } = new();

    public bool IsUser => Roles.Contains(SystemRoles.USER);
    public bool IsAdmin => Roles.Contains(SystemRoles.ADMIN);
}
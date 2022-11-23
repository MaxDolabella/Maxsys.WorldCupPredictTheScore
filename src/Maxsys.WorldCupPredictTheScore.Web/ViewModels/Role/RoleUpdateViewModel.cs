using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Role;

public class RoleUpdateViewModel
{
    public AppRole Role { get; set; }
    public IEnumerable<AppUser> Members { get; set; }
    public IEnumerable<AppUser> NonMembers { get; set; }
}
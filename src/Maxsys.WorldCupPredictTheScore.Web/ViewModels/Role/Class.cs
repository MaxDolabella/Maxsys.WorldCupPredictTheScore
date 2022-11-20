using System.ComponentModel.DataAnnotations;
using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Role;

public class RoleUpdateViewModel
{
    public AppRole Role { get; set; }
    public IEnumerable<AppUser> Members { get; set; }
    public IEnumerable<AppUser> NonMembers { get; set; }
}


public class RoleModificationViewModel
{
    [Required]
    public string RoleName { get; set; }

    public string RoleId { get; set; }

    public string[]? AddIds { get; set; }

    public string[]? DeleteIds { get; set; }
}


public class RoleListViewModel
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string RoleMembers { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Role;

public class RoleModificationViewModel
{
    [Required]
    public string RoleName { get; set; }

    public string RoleId { get; set; }

    public string[]? AddIds { get; set; }

    public string[]? DeleteIds { get; set; }
}
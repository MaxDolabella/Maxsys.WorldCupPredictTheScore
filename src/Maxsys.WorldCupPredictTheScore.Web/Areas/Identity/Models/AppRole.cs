using Microsoft.AspNetCore.Identity;

namespace Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;

public sealed class AppRole : IdentityRole<Guid>
{
    /// <summary>
    /// Initializes a new instance of <see cref="AppRole"/>.
    /// </summary>
    /// <remarks>
    /// The Id property is initialized to form a new GUID string value.
    /// </remarks>
    public AppRole()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="AppRole"/>.
    /// </summary>
    /// <param name="roleName">The role name.</param>
    public AppRole(string roleName) : base(roleName)
    {
        Id = Guid.NewGuid();
    }
}
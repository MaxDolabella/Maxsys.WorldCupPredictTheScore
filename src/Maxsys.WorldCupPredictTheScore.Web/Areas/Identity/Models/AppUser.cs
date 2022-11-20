using Microsoft.AspNetCore.Identity;

namespace Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;

public sealed class AppUser : IdentityUser<Guid>
{
	public AppUser() : base()
	{
		Id = Guid.NewGuid();
		SecurityStamp = Guid.NewGuid().ToString();
    }
}
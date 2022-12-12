using Maxsys.WorldCupPredictTheScore.Web.Models;

namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static bool HasRole(this ClaimsPrincipal user, string value)
    {
        return user.HasClaim(claim => claim.Type == ClaimTypes.Role && claim.Value == value);
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.HasRole(SystemRoles.ADMIN);
    }

    public static bool IsUser(this ClaimsPrincipal user)
    {
        return user.HasRole(SystemRoles.USER);
    }
}
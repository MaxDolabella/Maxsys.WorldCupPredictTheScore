using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Helpers;

[HtmlTargetElement("td", Attributes = "i-role")]
public class CustomTagHelpers : TagHelper
{
    private UserManager<AppUser> userManager;
    private RoleManager<AppRole> roleManager;

    public CustomTagHelpers(UserManager<AppUser> usermgr, RoleManager<AppRole> rolemgr)
    {
        userManager = usermgr;
        roleManager = rolemgr;
    }

    [HtmlAttributeName("i-role")]
    public string Role { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var names = new List<string>();
        var role = await roleManager.FindByIdAsync(Role);
        if (role != null)
        {
            foreach (var user in userManager.Users)
            {
                if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                    names.Add(user.UserName);
            }
        }
        output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
    }
}
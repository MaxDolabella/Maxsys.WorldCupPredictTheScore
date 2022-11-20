using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.WorldCupPredictTheScore.Web.Controllers;

[Authorize(Roles = "admin")]
[Route("roles")]
public class RoleController : Controller
{
    private readonly RoleManager<AppRole> roleManager;
    private readonly UserManager<AppUser> userManager;

    public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles = await roleManager.Roles
            .Select(role => new RoleListViewModel
            {
                RoleId = role.Id.ToString(),
                RoleName = role.Name,
                RoleMembers = string.Empty
            }).ToListAsync();

        foreach (var role in roles)
            role.RoleMembers = await ProcessAsync(role.RoleId);

        return View(roles);
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create")]
    public async Task<IActionResult> Create([Required] string name)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await roleManager.CreateAsync(new AppRole(name));
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                Errors(result);
        }
        return View(name);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                Errors(result);
        }
        else
            ModelState.AddModelError("", "No role found");
        return View("Index", roleManager.Roles);
    }

    [HttpGet("update/{id}")]
    public async Task<IActionResult> Update(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        var members = new List<AppUser>();
        var nonMembers = new List<AppUser>();
        foreach (AppUser user in userManager.Users)
        {
            var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
            list.Add(user);
        }
        return View(new RoleUpdateViewModel
        {
            Role = role,
            Members = members,
            NonMembers = nonMembers
        });
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromForm] RoleModificationViewModel model)
    {
        IdentityResult result;
        if (ModelState.IsValid)
        {
            foreach (string userId in model.AddIds ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await userManager.AddToRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
            foreach (string userId in model.DeleteIds ?? new string[] { })
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
        }

        if (ModelState.IsValid)
            return RedirectToAction(nameof(Index));
        else
            return await Update(model.RoleId);
    }

    public async Task<string> ProcessAsync(string roleId)
    {
        var names = new List<string>();
        var role = await roleManager.FindByIdAsync(roleId);
        if (role != null)
        {
            foreach (var user in userManager.Users)
            {
                if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                    names.Add(user.UserName);
            }
        }

        return names.Any() ? string.Join(", ", names) : "No Users";
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);
    }
}
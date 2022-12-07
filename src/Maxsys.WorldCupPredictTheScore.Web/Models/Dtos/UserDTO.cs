namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class UserDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Roles { get; set; } = new List<string>();

    public bool IsUser => Roles.Contains(SystemRoles.USER);
    public bool IsAdmin => Roles.Contains(SystemRoles.ADMIN);
}
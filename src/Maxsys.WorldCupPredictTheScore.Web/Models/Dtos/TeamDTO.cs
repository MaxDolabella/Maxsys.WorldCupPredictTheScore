namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

[System.Diagnostics.DebuggerDisplay("{Name}")]
public sealed class TeamDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}
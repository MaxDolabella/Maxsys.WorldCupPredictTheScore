namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class MatchCreateDTO
{
    public DateTime Date { get; set; }
    public char Group { get; set; }
    public byte Round { get; set; }

    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
}
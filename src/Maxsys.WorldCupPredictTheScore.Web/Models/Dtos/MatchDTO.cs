namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

[System.Diagnostics.DebuggerDisplay("{HomeTeam.Code} {HomeTeamScore} X {AwayTeamScore} {AwayTeam.Code}")]
public sealed class MatchDTO
{
    public Guid MatchId { get; set; }
    public DateTime Date { get; set; }
    public char Group { get; set; }
    public byte Round { get; set; }

    public TeamDTO HomeTeam { get; set; }
    public TeamDTO AwayTeam { get; set; }

    public byte? HomeTeamScore { get; set; }
    public byte? AwayTeamScore { get; set; }

    public string RoundToString()
    {
        return Round switch
        {
            4 => "Oitavas de final",
            5 => "Quartas de final",
            6 => "Semifinal",
            7 => "Decisão de 3º Lugar",
            8 => "FINAL",
            _ => $"Rodada {Round} / Grupo {Group}"
        };
    }
}
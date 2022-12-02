using System.ComponentModel.DataAnnotations;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

public sealed class MatchCreateViewModel
{
    private static Dictionary<byte, string> s_rounds = new Dictionary<byte, string>
    {
        [1] = "Rodada 1",
        [2] = "Rodada 2",
        [3] = "Rodada 3",
        [4] = "Oitavas de final",
        [5] = "Quartas de final",
        [6] = "Semifinal",
        [7] = "Decisão de 3º Lugar",
        [8] = "FINAL"
    };
    public IReadOnlyList<TeamViewModel> Teams { get; set; } = new List<TeamViewModel>();
    public Dictionary<byte, string> Rounds => s_rounds;


    [Required]
    [NotDefault("Data obrigatória.")]
    [Display(Name = "Data e hora do jogo (UTC).")]
    public DateTime? MatchDate { get; set; }

    [Required]
    [NotDefault("Time da casa obrigatório.")]
    [Display(Name = "Time Casa")]
    public Guid? SelectedHomeTeamId { get; set; }
    
    [Required]
    [NotDefault("Time visitante obrigatório.")]
    [Display(Name = "Time Visitante")]
    public Guid? SelectedAwayTeamId { get; set; }

    [Display(Name = "Grupo (pode ser deixado em branco)")]
    public char? Group { get; set; } = null;

    [Required]
    [Display(Name = "Rodada")]
    public byte? SelectedRound { get; set; }
}






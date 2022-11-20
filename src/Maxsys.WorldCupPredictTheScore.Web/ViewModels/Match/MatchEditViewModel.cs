using System.ComponentModel.DataAnnotations;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;

public sealed class MatchEditViewModel
{
    [Required]
    public Guid MatchId { get; set; }

    public string HomeTeamName { get; set; }
    public string AwayTeamName { get; set; }

    [Required]
    [Range(0,20)]
    public byte? HomeTeamScore { get; set; }
    
    [Required]
    [Range(0, 20)]
    public byte? AwayTeamScore { get; set; }
}

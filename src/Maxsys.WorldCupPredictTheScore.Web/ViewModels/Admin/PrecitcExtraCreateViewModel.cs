using System.ComponentModel.DataAnnotations;

namespace Maxsys.WorldCupPredictTheScore.Web.ViewModels.Admin;

public class PrecitcExtraCreateViewModel
{
    public IList<UserSelectViewModel> UsersSelect { get; set; } = new List<UserSelectViewModel>();
    public IList<MatchSelectViewModel> MatchesSelect { get; set; } = new List<MatchSelectViewModel>();

    [Required]
    public Guid? SelectedUserId { get; set; }

    [Required]
    public Guid? SelectedMatchId { get; set; }

    [Required]
    [Range(0, 20, ErrorMessage = "Campo obrigatório")]
    public byte? HomeTeamScore { get; set; }

    [Required]
    [Range(0, 20, ErrorMessage = "Campo obrigatório")]
    public byte? AwayTeamScore { get; set; }
}

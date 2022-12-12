namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

/// <summary>
/// Contém Match
/// </summary>
public sealed class UserPredictionsDTO
{
    public UserDTO User { get; set; }
    public IReadOnlyList<UserPredictionsItemDTO> Items { get; set; }
}

public sealed class UserPredictionsItemDTO
{
    public MatchDTO Match { get; set; }
    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }
    public int Points { get; set; }
}


/// <summary>
/// Contém Match
/// </summary>
public sealed class MatchPredictionsDTO
{
    public MatchDTO Match { get; set; }
    public IReadOnlyList<MatchPredictionsItemDTO> Items { get; set; }
}

public sealed class MatchPredictionsItemDTO
{   
    public UserDTO User { get; set; }
    public byte HomeTeamScore { get; set; }
    public byte AwayTeamScore { get; set; }
    public int? Points { get; set; }
}
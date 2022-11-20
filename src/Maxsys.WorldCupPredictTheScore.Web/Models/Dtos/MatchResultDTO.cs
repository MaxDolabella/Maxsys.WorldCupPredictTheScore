namespace Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;

public sealed class MatchResultDTO
{
    public MatchResultDTO(byte homeScore, byte awayScore)
    {
        HomeScore = homeScore;
        AwayScore = awayScore;

        var winner = homeScore > awayScore
                ? MatchWinner.HomeTeam
                : awayScore > homeScore
                    ? MatchWinner.AwayTeam
                    : MatchWinner.Draw;

        ScoreDifference = homeScore - awayScore;
        Winner = winner;
        WinnerScore = winner == MatchWinner.HomeTeam ? homeScore : awayScore;
        LoserScore = winner == MatchWinner.HomeTeam ? awayScore : homeScore;
    }

    public MatchWinner Winner { get; }
    public byte HomeScore { get; }
    public byte AwayScore { get; }
    public int ScoreDifference { get; }
    public byte WinnerScore { get; }
    public byte LoserScore { get; }
}
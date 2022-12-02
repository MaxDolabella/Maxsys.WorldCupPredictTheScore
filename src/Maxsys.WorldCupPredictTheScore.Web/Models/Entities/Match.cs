namespace Maxsys.WorldCupPredictTheScore.Web.Models.Entities;

public sealed class Match : Entity
{
    public Match()
    { }

    public Match(char group, byte round, DateTime date, Team homeTeam, Team awayTeam)
        : this(group, round, date, homeTeam.Id, awayTeam.Id)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
    }

    public Match(char group, byte round, DateTime date, Guid homeTeamId, Guid awayTeamId)
        : base(Guid.NewGuid())
    {
        Group = group;
        Round = round;
        Date = date;
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        HomeScore = null;
        AwayScore = null;
    }


    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }

    public char Group { get; set; }
    public byte Round { get; set; }
    public DateTime Date { get; set; }

    public Team HomeTeam { get; set; }
    public Team AwayTeam { get; set; }

    public byte? HomeScore { get; set; }
    public byte? AwayScore { get; set; }

    public MatchWinner? GetWinner()
    {
        if (HomeScore == null || AwayScore == null)
            return null;

        return HomeScore > AwayScore
            ? MatchWinner.HomeTeam
            : AwayScore > HomeScore
                ? MatchWinner.AwayTeam
                : MatchWinner.Draw;
    }
}
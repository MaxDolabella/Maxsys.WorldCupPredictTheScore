namespace Maxsys.WorldCupPredictTheScore.Web.Models.Entities;

public sealed class Team : Entity
{
    public Team(string name, string code)
    {
        Name = name;
        Code = code;
    }

    protected Team()
    { }

    public string Name { get; set; }
    public string Code { get; set; }
}
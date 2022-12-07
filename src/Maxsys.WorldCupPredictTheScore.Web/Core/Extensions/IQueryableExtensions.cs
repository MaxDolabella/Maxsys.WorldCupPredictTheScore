using System.Linq.Expressions;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;

public static class IQueryableExtensions
{
    /// <summary>
    /// Atalho para query.GroupJoin(...).SelectMany(...)
    /// </summary>
    /// <returns></returns>
    public static IQueryable<TResult> LeftOuterJoin<TSource, TInner, TKey, TJoinResult, TResult>(
        this IQueryable<TSource> outer,
        IQueryable<TInner> inner,
        Expression<Func<TSource, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<TSource, IEnumerable<TInner>, TJoinResult>> joinResultSelector,
        Expression<Func<TJoinResult, IEnumerable<TInner?>>> collectionSelector,
        Expression<Func<TJoinResult, TInner?, TResult>> resultSelector)
    {
        var query = outer
            .GroupJoin(inner, outerKeySelector, innerKeySelector, joinResultSelector)
            .SelectMany(collectionSelector, resultSelector);

        return query;
    }

    public static IQueryable<MatchDTO> SelectMatch(this IQueryable<Match> query)
    {
        return query.Select(m => new MatchDTO
        {
            MatchId = m.Id,
            Group = m.Group,
            Round = m.Round,
            Date = m.Date,
            HomeTeam = new TeamDTO
            {
                Id = m.HomeTeam.Id,
                Name = m.HomeTeam.Name,
                Code = m.HomeTeam.Code
            },
            AwayTeam = new TeamDTO
            {
                Id = m.AwayTeam.Id,
                Name = m.AwayTeam.Name,
                Code = m.AwayTeam.Code
            },
            HomeTeamScore = m.HomeScore,
            AwayTeamScore = m.AwayScore
        });
    }

    public static IQueryable<TeamDTO> SelectTeam(this IQueryable<Team> query)
    {
        return query.Select(m => new TeamDTO
        {
            Id = m.Id,
            Name = m.Name,
            Code = m.Code
        });
    }

    public static IQueryable<MatchPredictionsItemDTO> SelectPredict(this IQueryable<PredictScore> query, ApplicationDbContext context)
    {
        return query
            .Select(predicted => new MatchPredictionsItemDTO
            {
                User = new UserDTO
                {   
                    Id = predicted.UserId,
                    Email = predicted.User.Email,
                    Name = predicted.User.Email.Substring(0, predicted.User.Email.IndexOf('@')),
                    //Roles = context.Roles
                    //    .Where(r => context.UserRoles
                    //        .Where(ur => ur.UserId == predicted.UserId)
                    //        .Select(ur => ur.RoleId)
                    //        .Contains(r.Id))
                    //    .Select(role => role.Name)
                    //    .AsEnumerable()
                },
                HomeTeamScore = predicted.HomeTeamScore,
                AwayTeamScore = predicted.AwayTeamScore
            });
    }

    
}
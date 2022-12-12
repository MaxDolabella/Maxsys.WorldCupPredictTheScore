using System.Linq.Expressions;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.CodeAnalysis;

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


        /* Exemplo
            .LeftOuterJoin(context.Set<Location>(),
                tariff => tariff.LocationId,
                location => location.Id,
                (outer, innerList) => new { outer, innerList },
                a => a.innerList.DefaultIfEmpty(),
                (a, innerItem) => new { tariff = a.outer, location = innerItem })
        */

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

    public static IQueryable<MatchPredictionsItemDTO> SelectMatchPrediction(this IQueryable<PredictScore> query, ApplicationDbContext context)
    {
        return query
            .LeftOuterJoin(context.Results,
                outer => outer.Id,
                inner => inner.PredictId,
                (outer, innerList) => new { outer, innerList },
                a => a.innerList.DefaultIfEmpty(),
                (a, innerItem) => new { PredictScore = a.outer, PredictResult = innerItem })
            .Select(a => new MatchPredictionsItemDTO
            {
                User = new UserDTO
                {   
                    Id = a.PredictScore.UserId,
                    Email = a.PredictScore.User.Email,
                    Name = a.PredictScore.User.Email.Substring(0, a.PredictScore.User.Email.IndexOf('@'))
                },
                HomeTeamScore = a.PredictScore.HomeTeamScore,
                AwayTeamScore = a.PredictScore.AwayTeamScore,
                Points = a.PredictResult.Points
            });
    }

    public static IQueryable<MatchPredictionsItemDTO> SelectMatchPrediction(this IQueryable<PredictScore> query)
    {
        return query
            .Select(predicted => new MatchPredictionsItemDTO
            {
                User = new UserDTO
                {
                    Id = predicted.UserId,
                    Email = predicted.User.Email,
                    Name = predicted.User.Email.Substring(0, predicted.User.Email.IndexOf('@'))
                },
                HomeTeamScore = predicted.HomeTeamScore,
                AwayTeamScore = predicted.AwayTeamScore,
                Points = null
            });
    }

    public static IQueryable<UserPredictionsItemDTO> SelectUserPredictionsItem(this IQueryable<PredictResult> query)
    {
        return query
            .Select(pResult => new UserPredictionsItemDTO
            {
                Match = new MatchDTO
                {
                    MatchId = pResult.Predict.Match.Id,
                    Group = pResult.Predict.Match.Group,
                    Round = pResult.Predict.Match.Round,
                    Date = pResult.Predict.Match.Date,
                    HomeTeam = new TeamDTO
                    {
                        Id = pResult.Predict.Match.HomeTeam.Id,
                        Name = pResult.Predict.Match.HomeTeam.Name,
                        Code = pResult.Predict.Match.HomeTeam.Code
                    },
                    AwayTeam = new TeamDTO
                    {
                        Id = pResult.Predict.Match.AwayTeam.Id,
                        Name = pResult.Predict.Match.AwayTeam.Name,
                        Code = pResult.Predict.Match.AwayTeam.Code
                    },
                    HomeTeamScore = pResult.Predict.Match.HomeScore,
                    AwayTeamScore = pResult.Predict.Match.AwayScore
                },
                HomeTeamScore = pResult.Predict.HomeTeamScore,
                AwayTeamScore = pResult.Predict.AwayTeamScore,
                Points = pResult.Points
            });
    }


}
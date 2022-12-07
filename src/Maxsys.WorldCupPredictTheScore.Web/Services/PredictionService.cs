using AutoMapper;
using FluentValidation.Results;
using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Match;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public sealed class PredictionService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly MatchRepository _matchRepository;
    private readonly PredictionRepository _predictionRepository;
    private readonly PredictResultRepository _resultsRepository;

    public PredictionService(ApplicationDbContext context, PredictionRepository predictionRepository, MatchRepository matchRepository, IMapper mapper, PredictResultRepository resultsRepository)
    {
        _context = context;
        _predictionRepository = predictionRepository;
        _matchRepository = matchRepository;
        _mapper = mapper;
        _resultsRepository = resultsRepository;
    }

    /// <summary>
    /// Obtém o próximo jogo de cada time.
    /// </summary>
    public async Task<PredictViewModel> GetUserAvailableMatchesToPredictAsync(Guid userId, CancellationToken cancellation = default)
    {
        // Obter data (agora)
        var now = DateTime.UtcNow;

        // Obter id dos jogos palpitados.
        var predictedMatchIds = await _context.Predictions.AsNoTracking()
            .Where(predicted => predicted.UserId == userId)
            .Select(predicted => predicted.MatchId)
            .ToListAsync(cancellation);

        var allmatches = await _context.Matches.AsNoTracking()
            .Where(m => m.HomeScore == null && m.AwayScore == null) // jogos ainda não jogados
            .OrderBy(m => m.Date)
            .SelectMatch()
            .ToListAsync(cancellation);

        // próximo jogo de cada seleção
        var nextMatches = new List<MatchDTO>();
        foreach (var match in allmatches)
        {
            if (!nextMatches.Any(nm
                => nm.HomeTeam.Id == match.HomeTeam.Id || nm.AwayTeam.Id == match.HomeTeam.Id
                || nm.HomeTeam.Id == match.AwayTeam.Id || nm.AwayTeam.Id == match.AwayTeam.Id))
            {
                nextMatches.Add(match);
            }
        }

        var nextMatchesToPredict = nextMatches
            .Where(match => match.Date > now) // jogos não iniciados
            .Where(match => !predictedMatchIds.Contains(match.MatchId)) // Jogos ainda não palpitados
            .Select(match => _mapper.Map<PredictListDTO>(match))
            //.OrderBy(m => m.Date)
            .ToList();

        return new PredictViewModel
        {
            UserId = userId,
            List = _mapper.Map<IList<PredictListViewModel>>(nextMatchesToPredict)
        };
    }

    public async Task<ValidationResult> SaveUserPredictsAsync(IEnumerable<PredictScore> predicts, CancellationToken cancellation = default)
    {
        var validationResult = new ValidationResult();

        foreach (var predict in predicts)
        {
            var exists = await _context.Predictions.AsNoTracking().AnyAsync(p => p.UserId == predict.UserId && p.MatchId == predict.MatchId, cancellation);
            if (exists)
                return validationResult.AddError($"Já existe um palpite deste usuário para o jogo {predict.MatchId}");

            var entry = await _context.Predictions.AddAsync(predict, cancellation);
            if (entry.State != EntityState.Added)
                return validationResult.AddError($"Falha ao adicionar palpite para o jogo {predict.MatchId}");
        }

        try
        {
            _ = await _context.SaveChangesAsync(cancellation);
        }
        catch (Exception ex)
        {
            _ = validationResult.AddError($"Falha ao adicionar palpite para o jogo: {ex}");
        }

        return validationResult;
    }

    public async Task<PreviousNextMatchDTO> GetPreviousNextMatchesAsync(Guid currentMatchId, CancellationToken cancellation = default)
    {
        var matchesIds = await _matchRepository.GetMatchesIdsAsync(cancellation);

        var currentIndex = matchesIds.IndexOf(currentMatchId);

        return new PreviousNextMatchDTO
        {
            PreviousMatchId = currentIndex > 0 ? matchesIds[currentIndex - 1] : default(Guid?),
            NextMatchId = currentIndex < matchesIds.Count - 1 ? matchesIds[currentIndex + 1] : default(Guid?)
        };
    }

    public async Task<MatchPredictionsDTO?> GetMatchPredictionsAsync(Guid matchId, CancellationToken cancellation = default)
    {
        var match = await _context.Matches.AsNoTracking()
            .SelectMatch()
            .FirstOrDefaultAsync(m => m.MatchId == matchId, cancellation);

        // TODO exception aqui.
        // Por enquanto retorna lista vazia
        if (match is null)
            return null;

        var query = _context.Predictions.AsNoTracking()
            .Where(p => p.MatchId == matchId);

        var predictions = await query
            .OrderBy(p => p.User.UserName)
            .Select(p => new PredictedScoreDTO
            {
                PredictionId = p.Id,
                UserId = p.UserId,
                UserName = p.User.UserName,
                HomeTeamScore = p.HomeTeamScore,
                AwayTeamScore = p.AwayTeamScore
            })
            .ToListAsync(cancellation);

        return new MatchPredictionsDTO
        {
            Match = match,
            Predictions = predictions
        };
    }

    public async Task<MatchPredictionsDTO_new?> GetMatchPredictionsAsync2(Guid matchId, CancellationToken cancellation = default)
    {
        var match = await _matchRepository.GetByIdAsync(matchId, cancellation);
        if (match is null)
            return null;

        var predictions = await _predictionRepository.GetPredictionsByMatchAsync(matchId, cancellation);

        return new MatchPredictionsDTO_new
        {
            Match = match,
            Predictions = predictions
        };
    }

    public async Task<MatchPredictionsViewModel?> MatchPredictionsAsync(Guid matchId, Guid loggedUserId, CancellationToken cancellation = default)
    {
        // Obtém-se o jogo para se listar os palpites
        var match = await _matchRepository.GetByIdAsync(matchId, cancellation);
        if (match is null)
            return null;

        // Obtém-se os palpites desse jogo
        var predictions = await _predictionRepository.GetPredictionsByMatchAsync(matchId, cancellation);

        // Obtém-se os ids dos jogos imediatamente anterior e posterior
        var previousNextMatch = await GetPreviousNextMatchesAsync(matchId, cancellation);

        // Data do jogo maior que agora = não jogado
        var notPlayedMatch = match.Date > DateTime.UtcNow;

        var viewModel = new MatchPredictionsViewModel
        {
            PreviousMatchId = previousNextMatch?.PreviousMatchId,
            NextMatchId = previousNextMatch?.NextMatchId,
            Match = _mapper.Map<MatchViewModel>(match),
            Predictions = _mapper.Map<IReadOnlyList<PredictedScoreViewModel>>(predictions)
        };

        // Obter pontos da partida
        var matchResults = await _resultsRepository.GetMatchResultsAsync(matchId, cancellation);

        // caso tenha pontos (partida em andamento ou finalizada) exibir
        foreach (var predictionScore in viewModel.Predictions)
        {
            // não visualizar palpites de adversários onde a partida não começou
            if (notPlayedMatch && predictionScore.UserId != loggedUserId)
            {
                predictionScore.HomeTeamScore = "?";
                predictionScore.AwayTeamScore = "?";
            }

            var resultPoints = matchResults.SingleOrDefault(points => points.UserId == predictionScore.UserId);
            if (resultPoints is not null)
                predictionScore.Points = resultPoints.Points;
        }

        return viewModel;
    }
}
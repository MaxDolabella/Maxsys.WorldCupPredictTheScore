using AutoMapper;
using FluentValidation.Results;
using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Core.Repositories;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Predict;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels.Prediction;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Services;

public sealed class PredictionService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly MatchRepository _matchRepository;
    private readonly PredictionRepository _predictionRepository;
    private readonly PredictionResultRepository _resultsRepository;

    public PredictionService(ApplicationDbContext context, PredictionRepository predictionRepository, MatchRepository matchRepository, IMapper mapper, PredictionResultRepository resultsRepository)
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
        var predictedMatchesIds = await _predictionRepository.GetPredictedMatchIdsByUserAsync(userId, cancellation);

        var allmatches = await _context.Matches.AsNoTracking()
            .Where(m => m.HomeScore == null && m.AwayScore == null) // jogos ainda não jogados
            .Where(match => match.Date > now) // jogos não iniciados// jogos não iniciados
            .Where(match => !predictedMatchesIds.Contains(match.Id)) // Jogos ainda não palpitados
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
            .Select(match => _mapper.Map<PredictListDTO>(match))
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

    public async Task<MatchPredictionsViewModel?> MatchPredictionsAsync(Guid matchId, Guid loggedUserId, CancellationToken cancellation = default)
    {
        var matchPredictions = await _predictionRepository.GetMatchPredictionsAsync_new(matchId, cancellation);
        if (matchPredictions is null)
            return null;

        // Obtém-se os ids dos jogos imediatamente anterior e posterior
        var previousNextMatch = await GetPreviousNextMatchesAsync(matchId, cancellation);

        var viewModel = _mapper.Map<MatchPredictionsViewModel>(matchPredictions);
        _mapper.Map(previousNextMatch, viewModel);

        // Data do jogo maior que agora = não jogado
        viewModel.IsNotPlayedMatch = matchPredictions.Match.Date > DateTime.UtcNow;
        viewModel.LoggedUser = loggedUserId;

        return viewModel;
    }

    public async Task<UserPredictionsViewModel?> UserPredictionsAsync(Guid userId, CancellationToken cancellation = default)
    {
        var model = await _resultsRepository.GetUserPredictionsAsync(userId, cancellation);

        return model is not null
            ? _mapper.Map<UserPredictionsViewModel>(model)
            : null;
    }
}
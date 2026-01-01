using Microsoft.Extensions.Logging;
using Scores.Interfaces;
using Scores.Models;

namespace Scores.Factories
{
    public class SortingStrategyFactory : ISortingStrategyFactory
    {
        private readonly Dictionary<string, IScoreSortingStrategy> _strategies;
        private readonly ILogger<SortingStrategyFactory> _logger;

        public SortingStrategyFactory(
            IEnumerable<KeyValuePair<string, IScoreSortingStrategy>> strategies,
            ILogger<SortingStrategyFactory> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _strategies = new Dictionary<string, IScoreSortingStrategy>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var strategy in strategies)
            {
                _strategies[strategy.Key] = strategy.Value;
            }

            _logger.LogInformation("Initialized SortingStrategyFactory with {Count} strategies", _strategies.Count);
        }

        public IScoreSortingStrategy GetStrategy(string subject)
        {
            if (_strategies.TryGetValue(subject, out var strategy))
            {
                _logger.LogDebug("Found strategy for subject: {Subject}", subject);
                return strategy;
            }

            _logger.LogWarning("No strategy found for subject: {Subject}, using default strategy", subject);
            return new DefaultSortingStrategy();
        }

        public void RegisterStrategy(string subject, IScoreSortingStrategy strategy)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("Subject cannot be null or empty.", nameof(subject));
            }

            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            _strategies[subject] = strategy;
            _logger.LogInformation("Registered strategy for subject: {Subject}", subject);
        }
    }

    public class DefaultSortingStrategy : IScoreSortingStrategy
    {
        public List<StudentScore> SortScores(List<StudentScore> scores)
        {
            return scores;
        }
    }
}

// 3. Create a factory to get the right strategy
using Scores.Interfaces;
using Scores.Models;

namespace Scores.Factories
{
    public class SortingStrategyFactory
    {
        private readonly Dictionary<string, IScoreSortingStrategy> _strategies;

        public SortingStrategyFactory()
        {
            _strategies = new Dictionary<string, IScoreSortingStrategy>(StringComparer.OrdinalIgnoreCase)
            {
                { "English", new EnglishSortingStrategy() },
                { "Maths", new MathsSortingStrategy() },
                { "Science", new ScienceSortingStrategy() }
            };
        }

        public IScoreSortingStrategy GetStrategy(string subject)
        {
            if (_strategies.TryGetValue(subject, out var strategy))
            {
                return strategy;
            }

            // Return a default strategy that doesn't sort
            return new DefaultSortingStrategy();
        }

        public void RegisterStrategy(string subject, IScoreSortingStrategy strategy)
        {
            _strategies[subject] = strategy;
        }
    }

    // Default strategy for unknown subjects
    public class DefaultSortingStrategy : IScoreSortingStrategy
    {
        public List<StudentScore> SortScores(List<StudentScore> scores)
        {
            return scores; // No sorting
        }
    }
}
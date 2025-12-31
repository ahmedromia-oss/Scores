using Scores.Enums;
using Scores.Interfaces;
using Scores.Models;
using System;

namespace Scores.Strategies
{
    public class EnumSortingStrategy<TEnum> : IScoreSortingStrategy
        where TEnum : struct, Enum
    {
        private readonly Func<string, string>? _preprocessScore;

        public EnumSortingStrategy(Func<string, string>? preprocessScore = null)
        {
            _preprocessScore = preprocessScore;
        }

        public List<StudentScore> SortScores(List<StudentScore> scores)
        {
            var orderedScores = scores.OrderBy(s =>
            {
                string scoreValue = _preprocessScore != null ? _preprocessScore(s.Score) : s.Score;
                
                if (Enum.TryParse<TEnum>(scoreValue, true, out var enumValue))
                    return (int)(object)enumValue;
                return 0;
            });

            return orderedScores.Reverse().ToList();
        }
    }
}


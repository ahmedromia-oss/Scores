using Scores.Enums;
using Scores.Strategies;

public class ScienceSortingStrategy : EnumSortingStrategy<ScienceScore>
{
    public ScienceSortingStrategy() : base(preprocessScore: s => s.Replace(" ", "")) { }
}
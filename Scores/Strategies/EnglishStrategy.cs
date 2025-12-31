using Scores.Enums;
using Scores.Strategies;

public class EnglishSortingStrategy : EnumSortingStrategy<EnglishScore>
{
    public EnglishSortingStrategy() : base(preprocessScore: s => s.Replace(" ", "")) { }
}
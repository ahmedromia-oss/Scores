namespace Scores.Interfaces
{
    public interface ISortingStrategyFactory
    {
        IScoreSortingStrategy GetStrategy(string subject);
        void RegisterStrategy(string subject, IScoreSortingStrategy strategy);
    }
}


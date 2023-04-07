using Issueneter.Domain.Models;

namespace Issueneter.Filters.PredefinedFilters;

public class StateFilter : IFilter<PullRequest>, IFilter<Issue>
{
    private readonly int _value;

    public StateFilter(int value)
    {
        _value = value;
    }

    public bool Apply(PullRequest entity)
    {
        return true;
    }

    public bool Apply(Issue entity)
    {
        return true;
    }
}
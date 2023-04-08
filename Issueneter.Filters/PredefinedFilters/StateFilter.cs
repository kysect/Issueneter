using Issueneter.Domain.Models;

namespace Issueneter.Filters.PredefinedFilters;

public class StateFilter : IFilter<PullRequest>, IFilter<Issue>
{
    public int Value { get; set; }
    
    public bool Apply(PullRequest entity)
    {
        return (int)entity.State == Value;
    }
    
    public bool Apply(Issue entity)
    {
        return (int)entity.State == Value;
    }
}
using Issueneter.Domain.Models;

namespace Issueneter.Filters.PredefinedFilters;

public class AuthorFilter : IFilter<Issue>, IFilter<PullRequest>
{
    public string Value { get; set; }

    public AuthorFilter()
    {
        
    }

    public bool Apply(Issue entity)
    {
        return entity.Author == Value;
    }

    public bool Apply(PullRequest entity)
    {
        return entity.Author == Value;
    }
}
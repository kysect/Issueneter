using Issueneter.Domain.Models;

namespace Issueneter.Filters.PredefinedFilters;

public class AuthorFilter : IFilter<Issue>, IFilter<PullRequest>
{
    private string _value;

    public AuthorFilter()
    {
        
    }
    public AuthorFilter(string value)
    {
        _value = value;
    }

    public bool Apply(Issue entity)
    {
        return entity.Author == _value;
    }

    public bool Apply(PullRequest entity)
    {
        return entity.Author == _value;
    }
}
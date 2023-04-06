using Issueneter.Domain;

namespace Issueneter.Filters;

public interface IFilter<TEntity> where TEntity : IFilterable
{
    bool Apply(TEntity entity);
}
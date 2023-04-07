using Issueneter.Domain;
using Issueneter.Domain.Models;
using Issueneter.Github;

namespace Issueneter.Filters.PredefinedFilters;

public enum ComplexOperand
{
    And = 1,
    Or = 2
}

public class ComplexFilter<T> : IFilter<T>
    where T: IFilterable
{
    public IFilter<T> Left { get; }
    public IFilter<T> Right { get; }
    public ComplexOperand Operand { get; }
    
    public bool Apply(T entity)
    {
        if (Left.Apply(entity))
        {
            return Operand == ComplexOperand.Or || Right.Apply(entity);
        }

        return Operand == ComplexOperand.Or && Right.Apply(entity);
    }
}
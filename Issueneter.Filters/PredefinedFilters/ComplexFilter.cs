using Issueneter.Annotation;
using Issueneter.Domain;

namespace Issueneter.Filters.PredefinedFilters;

public enum ComplexOperand
{
    And = 1,
    Or = 2
}

public class ComplexFilter<T> : IFilter<T>
    where T: IFilterable
{
    public ComplexFilter(ComplexOperand operand)
    {
        Operand = operand;
    }

    public IFilter<T> Left { get; set; }
    public IFilter<T> Right { get; set; }
    public ComplexOperand Operand { get; set; }
    
    public bool Apply(T entity)
    {
        if (Left.Apply(entity))
        {
            return Operand == ComplexOperand.Or || Right.Apply(entity);
        }

        return Operand == ComplexOperand.Or && Right.Apply(entity);
    }
}
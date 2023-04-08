using Issueneter.Domain.Models;

namespace Issueneter.Filters.PredefinedFilters;

public enum LabelOperand
{
    Any = 1,
    All = 2
}

public class LabelFilter : IFilter<Issue>, IFilter<PullRequest>
{
    public LabelFilter(List<string> labels)
    {
        Labels = labels;
    }

    public List<string> Labels { get; set; }
    public LabelOperand Operand { get; set; }
    
    public bool Apply(Issue entity)
    {
        var checkEvents = Operand == LabelOperand.Any 
            ? Labels.Any(l => entity.Labels.Contains(l)) 
            : Labels.All(l => entity.Labels.Contains(l));
        
        if (!checkEvents)
            return false;
        
        var events = entity.Events.Load().GetAwaiter().GetResult();
        return events.Any(e => e is LabeledEvent le && Labels.Contains(le.Label));
    }

    public bool Apply(PullRequest entity)
    {
        var checkEvents = Operand == LabelOperand.Any 
            ? Labels.Any(l => entity.Labels.Contains(l)) 
            : Labels.All(l => entity.Labels.Contains(l));
        
        if (!checkEvents)
            return false;
        
        var events = entity.Events.Load().GetAwaiter().GetResult();
        return events.Any(e => e is LabeledEvent le && Labels.Contains(le.Label));
    }
}
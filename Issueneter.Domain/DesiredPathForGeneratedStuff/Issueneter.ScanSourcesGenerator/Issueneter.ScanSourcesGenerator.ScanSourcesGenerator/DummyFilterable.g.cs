using System;
using System.Collections.Generic;
using Issueneter.Annotation;
using Issueneter.Mappings;

namespace Issueneter.Domain.Models;

public partial class DummyFilterable : IFilterable
{
    public string GetProperty(string name) => name.ToLower() switch
    {
        _ => throw new ArgumentOutOfRangeException(nameof(name), $"Not expected property name: {name}"),
    };

    public static ScanType ScanType => ScanType.DummyFilterable;
}
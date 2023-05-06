
using System;
using System.Collections.Generic;

namespace Mappings;

public static class FromEntity
{
    public static Dictionary<string, string[]> Values { get; } = new()
    {
               ["Issue"] = new [] {"Aboba", "Author", "State", "Labels", "Events"},
       ["PullRequest"] = new [] {"Title", "Author", "Url", "State", "Labels", "Events"}
    };
}
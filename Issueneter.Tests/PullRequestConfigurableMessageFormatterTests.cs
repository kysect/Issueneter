using System.Collections;
using Issueneter.Domain.Models;
using Issueneter.Domain.Utility;
using Issueneter.Telegram.Formatters;

namespace Issueneter.Tests;

public class PullRequestConfigurableMessageFormatterTests
{
    private static IEnumerable GetTestCases()
    {
        var dummyRefValue = new Ref<List<TimelineEvent>>(() => Task.FromResult(new List<TimelineEvent>()));

        var issue = new PullRequest(
            "PR title",
            "PR author",
            "Url",
            PullRequestState.Opened,
            Array.Empty<string>(),
            dummyRefValue);

        yield return new TestCaseData("Message template", issue, "Message template");
        yield return new TestCaseData("Pull request {value.Title}", issue, $"Pull request {issue.Title}");
        yield return new TestCaseData("Pull request {value.Url}", issue, $"Pull request {issue.Url}");
    }

    [TestCaseSource(nameof(GetTestCases))]
    public void Formatter_ReturnCorrectString(string template, PullRequest pullRequest, string expected)
    {
        var formatter = new ConfigurableMessageFormatter<PullRequest>(template);

        var actual = formatter.ToMessage(pullRequest);

        Assert.That(actual, Is.EqualTo(expected));
    }
}
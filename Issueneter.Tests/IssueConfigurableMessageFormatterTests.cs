using System.Collections;
using Issueneter.Domain.Models;
using Issueneter.Domain.Utility;
using Issueneter.Telegram.Formatters;

namespace Issueneter.Tests
{
    public class IssueConfigurableMessageFormatterTests
    {
        private static IEnumerable GetTestCases()
        {
            var dummyRefValue = new Ref<List<TimelineEvent>>(() => Task.FromResult(new List<TimelineEvent>()));

            var issue = new Issue(
                "Issue title", 
                "Issue author", 
                "Url",
                IssueState.Opened, 
                Array.Empty<string>(), 
                dummyRefValue);

            yield return new TestCaseData("Message template", issue, "Message template");
            yield return new TestCaseData("Issue {value.Title}", issue, $"Issue {issue.Title}");
            yield return new TestCaseData("Issue {value.Url}", issue, $"Issue {issue.Url}");
        }
 
        [TestCaseSource(nameof(GetTestCases))]
        public void Formatter_ReturnCorrectString(string template, Issue issue, string expected)
        {
            var formatter = new ConfigurableMessageFormatter<Issue>(template);

            var actual = formatter.ToMessage(issue);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
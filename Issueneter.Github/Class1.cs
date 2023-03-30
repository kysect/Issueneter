using Octokit.GraphQL;
using static Octokit.GraphQL.Variable;

namespace Issueneter.Github;

public class Class1
{
    public static async Task Test()
    {
        var productInformation = new ProductHeaderValue("YOUR_PRODUCT_NAME", "YOUR_PRODUCT_VERSION");
        var connection = new Connection(productInformation, "YOUR_OAUTH_TOKEN");

            .RepositoryOwner(Var("owner"))
            .Repository(Var("name"))
            .Issues().Nodes
            .Select(repo => new
            {
                repo.Id,
                repo.Name,
                repo.Owner.Login,
                repo.IsFork,
                repo.IsPrivate,
            }).Compile();

        var vars = new Dictionary<string, object>
        {
            { "owner", "octokit" },
            { "name", "octokit.graphql.net" },
        };

        var result =  await connection.Run(query, vars);

        Console.WriteLine(result.Login + " & " + result.Name + " Rocks!");
    }
}
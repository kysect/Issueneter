using Issueneter.Github;
using Issueneter.Host.Composition;
using Octokit;

namespace Issueneter.Host.Modules;

public static class GithubModule
{
    public static IServiceCollection AddGithubModule(this IServiceCollection services, IHostEnvironment env, IConfigurationRoot configuration) 
        => services
            .Configure<GithubOptions>(configuration.GetSection(nameof(GithubOptions)))
            .AddSingleton<GithubApiService>()
            .AddSingleton<IGitHubClient>(sp =>
            {
                var productInformation = new ProductHeaderValue("ISSUENETER", "1.0.0");
                return new GitHubClient(productInformation)
                {
                    Credentials = new Credentials(sp.GetRequiredOptions<GithubOptions>().Token)
                };
            });
}
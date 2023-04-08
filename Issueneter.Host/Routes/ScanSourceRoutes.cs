using Issueneter.Host.Responses;

namespace Issueneter.Host.Routes;

public static class ScanSourceRoutes
{
    // TODO: Засурсгенить
    private static readonly Dictionary<string, AvailableSourceResponse[]> Availables = new()
    {
        ["Issue"] = new[] {new AvailableSourceResponse("Issue author", "CreatedBy"), new AvailableSourceResponse("Is open", "State")},
        ["PullRequest"] = new[] {new AvailableSourceResponse("Pull request author", "CreatedBy"), new AvailableSourceResponse("State", "State")}
    };

    public static WebApplication MapScanSourceRoutes(this WebApplication app)
    {
        app.MapGet("/available_sources", () => Availables).WithOpenApi();

        return app;
    }
}
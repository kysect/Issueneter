using Issueneter.Mappings;

namespace Issueneter.Host.Routes;

public static class ScanSourceRoutes
{
    public static WebApplication MapScanSourceRoutes(this WebApplication app)
    {
        app.MapGet("/available_sources", () => ModelsInfo.AvailableScanSources).WithOpenApi();

        return app;
    }
}
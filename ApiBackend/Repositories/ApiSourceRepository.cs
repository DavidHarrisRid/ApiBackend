// Enthält feste Beispielquellen für den PoC.
public sealed class ApiSourceRepository
{
    // Liste der aktuell bekannten Beispielquellen.
    private readonly List<ApiSource> _sources =
    [
        // Beispielquelle: Wetterdaten.
        new ApiSource
        {
            Id = 1,
            Name = "Open-Meteo München",
            BaseUrl = "https://api.open-meteo.com",
            Endpoint = "/v1/forecast",
            Method = "GET",
            QueryParameters = new Dictionary<string, string>
            {
                ["latitude"] = "48.1374",
                ["longitude"] = "11.5755",
                ["current"] = "temperature_2m,relative_humidity_2m,wind_speed_10m",
                ["timezone"] = "Europe/Berlin"
            }
        },

        // Beispielquelle: einfache Test-API.
        new ApiSource
        {
            Id = 2,
            Name = "JSONPlaceholder Posts",
            BaseUrl = "https://jsonplaceholder.typicode.com",
            Endpoint = "/posts",
            Method = "GET",
            QueryParameters = new Dictionary<string, string>
            {
                ["userId"] = "1"
            }
        },

        // Beispielquelle: Transportdaten.
        new ApiSource
        {
            Id = 3,
            Name = "VBB Transport Radar",
            BaseUrl = "https://v6.vbb.transport.rest",
            Endpoint = "/radar",
            Method = "GET",
            QueryParameters = new Dictionary<string, string>
            {
                ["north"] = "52.52411",
                ["west"] = "13.41002",
                ["south"] = "52.51942",
                ["east"] = "13.41709"
            }
        }
    ];

    // Gibt alle Beispielquellen zurück.
    public IReadOnlyList<ApiSource> GetAll()
    {
        // Gibt die interne Liste nur lesend zurück.
        return _sources;
    }

    // Sucht eine Quelle anhand ihrer ID.
    public ApiSource? GetById(int id)
    {
        // Gibt die erste passende Quelle oder null zurück.
        return _sources.FirstOrDefault(source => source.Id == id);
    }
}
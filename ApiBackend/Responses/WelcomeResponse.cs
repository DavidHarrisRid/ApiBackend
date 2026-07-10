// Baut die einfache Startseiten-Antwort.
public static class WelcomeResponse
{
    // Erstellt die API-Übersicht für GET /.
    public static object Create()
    {
        // Gibt eine kompakte Beschreibung der API zurück.
        return new
        {
            message = "Data Aggregator API Backend läuft.",

            purpose = "Externe APIs können über eine konfigurierbare Request-Struktur abgerufen werden.",

            availableEndpoints = new[]
            {
                "GET /",
                "GET /health",
                "GET /api/sources",
                "GET /api/sources/1/fetch",
                "GET /api/sources/2/fetch",
                "GET /api/sources/3/fetch",
                "POST /api/fetch"
            },

            minimalApiSourceRequest = new
            {
                name = "Name der API-Quelle",
                baseUrl = "https://example.com",
                endpoint = "/api/example",
                method = "GET",
                queryParameters = new Dictionary<string, string>
                {
                    ["parameterName"] = "parameterValue"
                }
            },

            openMeteoExample = new
            {
                name = "Open-Meteo München",
                baseUrl = "https://api.open-meteo.com",
                endpoint = "/v1/forecast",
                method = "GET",
                queryParameters = new Dictionary<string, string>
                {
                    ["latitude"] = "48.1374",
                    ["longitude"] = "11.5755",
                    ["current"] = "temperature_2m,relative_humidity_2m,wind_speed_10m",
                    ["timezone"] = "Europe/Berlin"
                }
            },

            transportExample = new
            {
                name = "VBB Transport Radar",
                baseUrl = "https://v6.vbb.transport.rest",
                endpoint = "/radar",
                method = "GET",
                queryParameters = new Dictionary<string, string>
                {
                    ["north"] = "52.52411",
                    ["west"] = "13.41002",
                    ["south"] = "52.51942",
                    ["east"] = "13.41709"
                }
            },

            postmanHint = "POST /api/fetch mit Body raw JSON und Content-Type application/json testen."
        };
    }
}
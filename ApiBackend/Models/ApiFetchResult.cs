using System.Text.Json.Nodes;

// Ergebnis eines externen API-Abrufs.
public sealed class ApiFetchResult
{
    // ID der verwendeten Quelle.
    public int SourceId { get; init; }

    // Name der verwendeten Quelle.
    public string SourceName { get; init; } = "";

    // Tatsächlich aufgerufene URL.
    public string RequestUrl { get; init; } = "";

    // HTTP-Statuscode der externen API.
    public int StatusCode { get; init; }

    // Gibt an, ob der Request erfolgreich war.
    public bool Success { get; init; }

    // Dauer des Abrufs in Millisekunden.
    public long DurationMilliseconds { get; init; }

    // JSON-Antwort, falls parsebar.
    public JsonNode? JsonData { get; init; }

    // Rohantwort, falls kein JSON zurückkam.
    public string? RawBody { get; init; }

    // Fehlermeldung, falls etwas schiefging.
    public string? Error { get; init; }

    // Zeitpunkt des Abrufs.
    public DateTime FetchedAtUtc { get; init; } = DateTime.UtcNow;
}
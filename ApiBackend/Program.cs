using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

// Registriert einen zentral verwalteten HttpClient für externe APIs.
builder.Services.AddHttpClient("ExternalApiClient", client =>
{
    // Setzt ein Timeout für externe API-Anfragen.
    client.Timeout = TimeSpan.FromSeconds(50);

    // Setzt einen einfachen User-Agenten für externe APIs.
    client.DefaultRequestHeaders.UserAgent.ParseAdd("DataAggregatorPoC/1.0");
});

// Registriert das Repository mit den Beispielquellen.
builder.Services.AddSingleton<ApiSourceRepository>();

// Registriert den Service für externe API-Requests.
builder.Services.AddScoped<ApiRequestService>();

// Erstellt die WebApplication.
var app = builder.Build();

// Startseite mit kurzer API-Übersicht.
app.MapGet("/", () =>
{
    // Gibt die ausgelagerte Welcome-Beschreibung zurück.
    return Results.Ok(WelcomeResponse.Create());
});

// Health-Endpunkt zum Prüfen, ob das Backend läuft.
app.MapGet("/health", () =>
{
    // Gibt einen einfachen Status zurück.
    return Results.Ok(new
    {
        status = "ok",
        timestampUtc = DateTime.UtcNow
    });
});

// Gibt alle vordefinierten API-Quellen zurück.
app.MapGet("/api/sources", (ApiSourceRepository repository) =>
{
    // Holt die Quellen aus dem Repository.
    return Results.Ok(repository.GetAll());
});

// Führt eine vordefinierte API-Quelle aus.
app.MapGet("/api/sources/{id:int}/fetch",
    async (
        int id, 
        ApiSourceRepository repository,
        ApiRequestService apiRequestService,
        CancellationToken cancellationToken) =>
    {
        // Sucht die API-Quelle anhand der ID.
        ApiSource? source = repository.GetById(id);

        // Gibt 404 zurück, falls die ID nicht existiert.
        if (source is null)
        {
            return Results.NotFound(new
            {
                error = $"Keine ApiSource mit ID {id} gefunden."
            });
        }

        // Führt den externen API-Request aus.
        ApiFetchResult result =
            await apiRequestService.FetchAsync(source, cancellationToken);

        // Gibt das Ergebnis zurück.
        return Results.Ok(result);
    });

// Führt eine frei definierte API-Quelle testweise aus.
app.MapPost("/api/fetch",
    async (
        ApiSource request,
        ApiRequestService apiRequestService,
        CancellationToken cancellationToken) =>
    {
        // Wandelt den Request in eine interne temporäre ApiSource um.
        ApiSource source = request;

        // Führt den externen API-Request aus.
        ApiFetchResult result =
            await apiRequestService.FetchAsync(source, cancellationToken);

        // Gibt das Ergebnis zurück.
        return Results.Ok(result);
    });

// Startet die API.
app.Run();
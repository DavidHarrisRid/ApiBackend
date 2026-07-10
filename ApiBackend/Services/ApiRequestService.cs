using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;

// Führt externe API-Anfragen aus.
public sealed class ApiRequestService
{
    // Factory für zentral verwaltete HttpClients.
    private readonly IHttpClientFactory _httpClientFactory;

    // Bekommt die HttpClientFactory per Dependency Injection.
    public ApiRequestService(IHttpClientFactory httpClientFactory)
    {
        // Speichert die Factory für spätere Requests.
        _httpClientFactory = httpClientFactory;
    }

    // Führt eine ApiSource als HTTP-Request aus.
    public async Task<ApiFetchResult> FetchAsync(
        ApiSource source,
        CancellationToken cancellationToken = default)
    {
        // Misst die Dauer des API-Abrufs.
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Speichert die fertige URL für das Ergebnis.
        string requestUrl = "";

        try
        {
            // Baut die vollständige URL aus BaseUrl, Endpoint und Query-Parametern.
            Uri uri = BuildUri(source);

            // Speichert die URL als Text.
            requestUrl = uri.ToString();

            // Baut die HttpRequestMessage.
            using HttpRequestMessage request = BuildRequest(source, uri);

            // Holt den zentral registrierten HttpClient.
            HttpClient client = _httpClientFactory.CreateClient("ExternalApiClient");

            // Sendet den Request an die externe API.
            using HttpResponseMessage response =
                await client.SendAsync(request, cancellationToken);

            // Liest die Antwort als Text.
            string body =
                await response.Content.ReadAsStringAsync(cancellationToken);

            // Stoppt die Zeitmessung.
            stopwatch.Stop();

            // Versucht, die Antwort als JSON zu lesen.
            JsonNode? jsonData = TryParseJson(body);

            // Gibt ein strukturiertes Ergebnis zurück.
            return new ApiFetchResult
            {
                SourceId = source.Id,
                SourceName = source.Name,
                RequestUrl = requestUrl,
                StatusCode = (int)response.StatusCode,
                Success = response.IsSuccessStatusCode,
                DurationMilliseconds = stopwatch.ElapsedMilliseconds,
                JsonData = jsonData,
                RawBody = jsonData is null ? body : null,
                Error = response.IsSuccessStatusCode ? null : response.ReasonPhrase
            };
        }
        catch (Exception exception)
        {
            // Stoppt die Zeitmessung auch bei Fehlern.
            stopwatch.Stop();

            // Gibt Fehler strukturiert zurück.
            return new ApiFetchResult
            {
                SourceId = source.Id,
                SourceName = source.Name,
                RequestUrl = requestUrl,
                StatusCode = 0,
                Success = false,
                DurationMilliseconds = stopwatch.ElapsedMilliseconds,
                Error = exception.Message
            };
        }
    }

    // Baut die vollständige URL aus ApiSource-Daten.
    private static Uri BuildUri(ApiSource source)
    {
        // Entfernt abschließende Slashes aus der BaseUrl.
        string baseUrl = source.BaseUrl.TrimEnd('/');

        // Entfernt führende Slashes aus dem Endpoint.
        string endpoint = source.Endpoint.TrimStart('/');

        // Kombiniert BaseUrl und Endpoint.
        string fullUrl = string.IsNullOrWhiteSpace(endpoint)
            ? baseUrl
            : $"{baseUrl}/{endpoint}";

        // Erstellt einen UriBuilder für Query-Parameter.
        UriBuilder builder = new(fullUrl);

        // Gibt die URL direkt zurück, falls es keine Query-Parameter gibt.
        if (source.QueryParameters.Count == 0)
        {
            return builder.Uri;
        }

        // Baut die Query-Parameter URL-sicher zusammen.
        string query = string.Join("&",
            source.QueryParameters.Select(parameter =>
                $"{WebUtility.UrlEncode(parameter.Key)}={WebUtility.UrlEncode(parameter.Value)}"));

        // Setzt die Query-Parameter an die URL.
        builder.Query = query;

        // Gibt die fertige URL zurück.
        return builder.Uri;
    }

    // Baut den technischen HTTP-Request.
    private static HttpRequestMessage BuildRequest(ApiSource source, Uri uri)
    {
        // Erstellt die HTTP-Methode.
        HttpMethod method = new(source.Method);

        // Erstellt den Request mit Methode und URL.
        HttpRequestMessage request = new(method, uri);

        // Fügt alle Header aus der ApiSource hinzu.
        foreach ((string key, string value) in source.Headers)
        {
            request.Headers.TryAddWithoutValidation(key, value);
        }

        // Prüft, ob ein Body gesetzt wurde.
        if (!string.IsNullOrWhiteSpace(source.Body))
        {
            request.Content = new StringContent(
                source.Body,
                Encoding.UTF8,
                "application/json");
        }

        // Gibt den fertigen Request zurück.
        return request;
    }

    // Versucht, Text als JSON zu lesen.
    private static JsonNode? TryParseJson(string body)
    {
        try
        {
            // Gibt JSON zurück, wenn der Body gültiges JSON ist.
            return JsonNode.Parse(body);
        }
        catch
        {
            // Gibt null zurück, wenn der Body kein JSON ist.
            return null;
        }
    }
}
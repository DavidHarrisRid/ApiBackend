// Interne Beschreibung einer externen API-Quelle.
public sealed class ApiSource
{
    // Interne ID der Quelle.
    public int Id { get; set; }

    // Lesbarer Name der Quelle.
    public string Name { get; set; } = "";

    // Basisadresse der externen API.
    public string BaseUrl { get; set; } = "";

    // Pfad innerhalb der externen API.
    public string Endpoint { get; set; } = "";

    // HTTP-Methode, z. B. GET oder POST.
    public string Method { get; set; } = "GET";

    // Optionale HTTP-Header.
    public Dictionary<string, string> Headers { get; set; } = new();

    // Query-Parameter für die URL.
    public Dictionary<string, string> QueryParameters { get; set; } = new();

    // Optionaler Body für POST, PUT oder PATCH.
    public string? Body { get; set; }
}
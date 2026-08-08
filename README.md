\# PoC 3 – Konfigurierbare API-Abfragen



Dieser PoC testet externe API-Abfragen über eine ASP.NET Core Minimal API und `IHttpClientFactory`.



\## Test



Im Ordner dieser README ein Terminal öffnen:



```powershell

dotnet run --project ApiBackend/ApiBackend.csproj --launch-profile http

```



Anschließend im Browser öffnen:



\- http://localhost:5105/health

\- http://localhost:5105/api/sources

\- http://localhost:5105/api/sources/1/fetch



Das Programm mit `Strg + C` beenden.



\## Erwartetes Ergebnis



Das Backend läuft, zeigt die konfigurierten Quellen und ruft Wetterdaten über eine externe API ab.


## BiometricService (Tatvik TMF20 bridge + Fingerprint microservice)

### What this is
- A bridge and microservice that enables hostel gate devices to capture and match fingerprints using the Tatvik TMF20 scanner, and integrates with a central web app to approve/reject outings and log in/out times.

### Components
- TMF20Bridge (net48)
  - Small .NET Framework 4.8 console app exposing the Tatvik SDK over HTTP on the local machine.
  - Endpoints: `/device/check`, `/device/info`, `/fingerprint/capture`, `/fingerprint/match`.
  - Requires `TMF20SDK.dll` and `TMF20Driver.dll` placed beside `TMF20Bridge.exe`.

- FingerprintService (net8)
  - ASP.NET Core Web API that talks to the bridge, stores templates in Firestore, and calls the web app to verify permissions and log time.
  - Endpoints (prefix `api/biometric`): `check-device`, `device-info`, `capture`, `enroll`, `verify/{studentId}`.

### How it works (high-level)
1) Gate PC hosts TMF20Bridge (talks to the device/SDK) and FingerprintService (talks to bridge + Firebase + WebApp).
2) Enroll: capture via bridge → store Base64 ISO template(s) in Firestore under `students/{studentId}/fingerprints/*`.
3) Verify: capture claim via bridge → compare to stored templates → if match, call WebApp to validate permission and log out/in, return consolidated result for the gate display.

### Prerequisites
- Windows 10/11 x64 on gate PC.
- .NET 8 SDK for FingerprintService.
- .NET Framework 4.8 Developer Pack for TMF20Bridge.
- Tatvik TMF20 hardware and official SDK DLLs: `TMF20SDK.dll`, `TMF20Driver.dll`.
- Firebase project with Firestore enabled; a service account JSON.
- WebApp endpoint to validate and log permissions (server-to-server API), e.g. `POST /api/permissions/check-and-log`.

### Clone
```bash
git clone https://github.com/sharmavaibhav31/biometricValidationSystem.git
cd BiometricService
```

### Configuration
- FingerprintService `appsettings.json` or `appsettings.Production.json`:
```json
{
  "Firebase": {
    "ProjectId": "<your-project-id>",
    "CredentialsPath": "<optional-full-path-or-use-env>"
  },
  "PermissionApi": {
    "BaseUrl": "http://<webapp-host>:<port>",
    "ApiKey": "<shared-secret>"
  }
}
```
- Alternatively set environment variables (PowerShell):
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:TMF20_BRIDGE_URL = "http://127.0.0.1:5010"
$env:GOOGLE_APPLICATION_CREDENTIALS = "D:\path\to\service-account.json"
```

### Build and run (local dev)
1) Build and run the bridge (requires net48 dev pack)
```powershell
cd .\TMF20-DotNetSDK-v1.0.4\src\TMF20Bridge
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe' TMF20Bridge.csproj /p:Configuration=Release
Start-Process -FilePath '.\bin\Release\TMF20Bridge.exe'
```
Ensure the following are in the same folder as `TMF20Bridge.exe`:
- `TMF20SDK.dll`
- `TMF20Driver.dll`

2) Run the API
```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:TMF20_BRIDGE_URL = "http://127.0.0.1:5010"
# Or: $env:GOOGLE_APPLICATION_CREDENTIALS = "D:\path\to\service-account.json"
cd ..\..\FingerprintService
dotnet run
```

### API endpoints (FingerprintService)
- GET `api/biometric/check-device` → `{ connected: true|false }`
- GET `api/biometric/device-info` → device info
- GET `api/biometric/capture` → `{ success: true, template: "<BASE64>" }`
- POST `api/biometric/enroll` `{ "StudentId": "S123" }` → saves template(s) to Firestore
- POST `api/biometric/verify/{studentId}` → captures, matches, calls WebApp, returns consolidated result

Example consolidated verify response:
```json
{
  "matched": true,
  "studentId": "S123",
  "permissionStatus": "Approved",
  "logged": true,
  "timestampUtc": "2025-08-27T09:55:00Z",
  "message": "OUT recorded"
}
```

### Integrating with the WebApp
- The service calls your WebApp: `POST {BaseUrl}/api/permissions/check-and-log` with body `{ studentId, direction }`.
- Configure `PermissionApi:BaseUrl` and `PermissionApi:ApiKey` in `appsettings.*`.
- The WebApp should:
  - Validate the student’s current approved permission (business rules/constraints below).
  - Log out/in time accordingly (idempotent per direction within a small window).
  - Return a small JSON: `{ studentId, permissionStatus, logged, message, timestampUtc }`.

### Front-end/gate device flow
1) For OUT: operator selects student (or scans QR/ID), then calls `verify/{studentId}`.
2) Display message based on JSON (Approved/Rejected) with timestamp.
3) For IN: repeat `verify/{studentId}` with direction inferred by server (or pass direction if needed).

### Firestore data shape
- `students/{studentId}/fingerprints/{autoId}` → `{ template: <BASE64>, createdAt: serverTimestamp }`
- Recommended: store 2–4 templates per student to improve match rate.

### Constraints and validation (suggested)
- PermissionStatus must be Approved at verification time window.
- Rate limiting: prevent multiple logs for the same student within N seconds.
- Direction inference: if last log is OUT, next verified event is IN (and vice versa); or pass explicit direction.
- Handle capture failures: retry after a short delay; prompt for better finger placement.

### Troubleshooting
- Device not detected:
  - Check `TMF20Driver.dll` exists and the Tatvik driver/service is installed/running.
  - Run the vendor demo app in `TMF20-DotNetSDK-v1.0.4/bin64/` to confirm hardware.
- 411 Length Required while testing bridge:
  - Use PowerShell `Invoke-RestMethod` or send curl body in one line or via `--data-binary @-`.
- Firebase credential errors:
  - Ensure `GOOGLE_APPLICATION_CREDENTIALS` or `Firebase:CredentialsPath` points to the JSON file.
  - Verify `Firebase:ProjectId` matches your project.

### Production deployment (gate PC)
- Bridge
  - Keep bound to `127.0.0.1` only.
  - Autostart via Task Scheduler or NSSM. Place `TMF20SDK.dll` and `TMF20Driver.dll` with the exe.
- API
  - Publish:
    ```powershell
    cd .\FingerprintService
    dotnet publish -c Release -r win-x64 --self-contained false
    ```
  - Run as Windows service (NSSM/sc). Set env vars:
    - `ASPNETCORE_ENVIRONMENT=Production`
    - `TMF20_BRIDGE_URL=http://127.0.0.1:5010`
    - `GOOGLE_APPLICATION_CREDENTIALS=<full path>` (or use appsettings.Production.json)
  - Security: bind to localhost or restrict via firewall; enable HTTPS if remote callers.

### Repository hygiene
- Do not commit service account JSON or `bin/` / `obj/` folders.
- Include `.gitignore` for .NET.
- Include a note or script to place Tatvik DLLs if license forbids committing them.

### Quick CLI tests
Bridge:
```powershell
curl.exe -s http://127.0.0.1:5010/device/check
curl.exe -s http://127.0.0.1:5010/fingerprint/capture
```
API (replace port):
```powershell
curl.exe -s http://localhost:5065/api/biometric/check-device
curl.exe -s -X POST "http://localhost:5065/api/biometric/enroll" -H "Content-Type: application/json" -d '{"StudentId":"S123"}'
curl.exe -s -X POST "http://localhost:5065/api/biometric/verify/S123"
```

### Notes
- The Tatvik SDK provided is .NET Framework oriented; the bridge isolates that and keeps the main API on .NET 8.
- For best reliability, enroll multiple templates and guide users on finger placement.



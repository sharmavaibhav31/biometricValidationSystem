using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.OpenApi.Models;
using FingerprintService.Services;
using FingerprintService.Storage;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var firebaseCredPath = builder.Configuration["Firebase:CredentialsPath"];
var firestoreProjectId = builder.Configuration["Firebase:ProjectId"];

// Firebase initialization (idempotent)
if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions
    {
        Credential = string.IsNullOrWhiteSpace(firebaseCredPath)
            ? GoogleCredential.GetApplicationDefault()
            : GoogleCredential.FromFile(firebaseCredPath)
    });
}

// Firestore
builder.Services.AddSingleton(provider => new FirestoreDbBuilder
{
    ProjectId = firestoreProjectId,
    Credential = FirebaseApp.DefaultInstance.Options.Credential
}.Build());

// Services
builder.Services.AddSingleton<ITatvikFingerprintService, TatvikFingerprintService>();
builder.Services.AddSingleton<IFingerprintRepository, FirebaseFingerprintRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fingerprint Service", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.OpenApi.Models;
using FingerprintService.Services;
using FingerprintService.Storage;
using FingerprintService.Web;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var envFirebaseCred = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
var firebaseCredPath = string.IsNullOrWhiteSpace(envFirebaseCred)
    ? builder.Configuration["Firebase:CredentialsPath"]
    : envFirebaseCred;
var firestoreProjectId = builder.Configuration["Firebase:ProjectId"];

// Firebase initialization (idempotent)
if (FirebaseApp.DefaultInstance == null)
{
    GoogleCredential credential;
    if (!string.IsNullOrWhiteSpace(firebaseCredPath) && File.Exists(firebaseCredPath))
    {
        credential = GoogleCredential.FromFile(firebaseCredPath);
    }
    else
    {
        credential = GoogleCredential.GetApplicationDefault();
    }

    FirebaseApp.Create(new AppOptions { Credential = credential });
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
builder.Services.AddHttpClient<IPermissionApiClient, PermissionApiClient>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fingerprint Service", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Production hardening
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();

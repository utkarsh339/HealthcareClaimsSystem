using HealthcareClaims.API.Middleware;
using HealthcareClaims.Application;
using HealthcareClaims.Application.Common.Authentication;
using HealthcareClaims.Domain.Enums;
using HealthcareClaims.Infrastructure;
using HealthcareClaims.Persistence;
using HealthcareClaims.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

foreach (var jsonSource in builder.Configuration.Sources
    .OfType<Microsoft.Extensions.Configuration.Json.JsonConfigurationSource>())
{
    jsonSource.ReloadOnChange = false;
}

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// CORS is configured through Cors:AllowedOrigins so production domains are not hard-coded.
var allowedOrigins = builder.Configuration["Cors:AllowedOrigins"]?
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    ?? Array.Empty<string>();

if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "No CORS origins configured. Set Cors:AllowedOrigins or Cors__AllowedOrigins.");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Application Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistence(builder.Configuration);

// JWT Settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtSecret = jwtSettings["SecretKey"];

if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret.Length < 32)
{
    throw new InvalidOperationException(
        "JwtSettings:SecretKey must be configured and contain at least 32 characters.");
}

builder.Services.Configure<JwtSettings>(jwtSettings);

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ClaimsProcessorOnly", policy => policy.RequireRole("ClaimsProcessor"));
    options.AddPolicy("MedicalReviewerOnly", policy => policy.RequireRole("MedicalReviewer"));
    options.AddPolicy("ProviderStaffOnly", policy => policy.RequireRole("ProviderStaff"));

    options.AddPolicy("PatientReadAccess", policy => policy.RequireRole(
        "Admin", "ClaimsProcessor", "MedicalReviewer", "ProviderStaff"));
    options.AddPolicy("PatientWriteAccess", policy => policy.RequireRole("Admin", "ProviderStaff"));

    options.AddPolicy("ProviderReadAccess", policy => policy.RequireRole(
        "Admin", "ClaimsProcessor", "MedicalReviewer", "ProviderStaff"));
    options.AddPolicy("ProviderWriteAccess", policy => policy.RequireRole("Admin", "ProviderStaff"));

    options.AddPolicy("InsurancePolicyReadAccess", policy => policy.RequireRole(
        "Admin", "ClaimsProcessor", "MedicalReviewer", "ProviderStaff"));
    options.AddPolicy("InsurancePolicyWriteAccess", policy => policy.RequireRole("Admin", "ProviderStaff"));

    options.AddPolicy("PriorAuthorizationReadAccess", policy => policy.RequireRole(
        UserRole.Admin.ToString(), UserRole.ClaimsProcessor.ToString(),
        UserRole.MedicalReviewer.ToString(), UserRole.ProviderStaff.ToString()));
    options.AddPolicy("PriorAuthorizationWriteAccess", policy => policy.RequireRole(
        UserRole.Admin.ToString(), UserRole.ProviderStaff.ToString(), UserRole.MedicalReviewer.ToString()));

    options.AddPolicy("ClaimReadAccess", policy => policy.RequireRole(
        UserRole.Admin.ToString(), UserRole.ClaimsProcessor.ToString(),
        UserRole.MedicalReviewer.ToString(), UserRole.ProviderStaff.ToString()));
    options.AddPolicy("ClaimWriteAccess", policy => policy.RequireRole(
        UserRole.Admin.ToString(), UserRole.ProviderStaff.ToString(), UserRole.ClaimsProcessor.ToString()));
    options.AddPolicy("ClaimProcessingAccess", policy => policy.RequireRole(
        UserRole.Admin.ToString(), UserRole.ClaimsProcessor.ToString()));

    options.AddPolicy("PaymentReadAccess", policy => policy.RequireRole(
        "Admin", "ClaimsProcessor", "MedicalReviewer", "ProviderStaff"));
    options.AddPolicy("PaymentWriteAccess", policy => policy.RequireRole("Admin", "ClaimsProcessor"));

    options.AddPolicy("DashboardReadAccess", policy => policy.RequireRole(
        UserRole.Admin.ToString(), UserRole.ClaimsProcessor.ToString(),
        UserRole.MedicalReviewer.ToString(), UserRole.ProviderStaff.ToString()));

    options.AddPolicy("AuditLogReadAccess", policy => policy.RequireRole(
        "Admin", "ClaimsProcessor", "MedicalReviewer"));
});

var app = builder.Build();

// Initialize a fresh deployment database from the EF Core model when enabled.
// This is intentionally used for the first PostgreSQL deployment because the previous
// migrations were SQL Server provider-specific. Future schema changes should use new
// PostgreSQL migrations instead of relying on EnsureCreated.
var ensureCreated = bool.TryParse(
    builder.Configuration["Database:EnsureCreated"],
    out var ensureCreatedValue) && ensureCreatedValue;

if (ensureCreated)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
}

// Global Exception Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Render terminates TLS at the edge. Local development can still use HTTPS redirection.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "HealthcareClaims.API",
    environment = app.Environment.EnvironmentName
}));

app.MapGet("/health/db", async (ApplicationDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();

    return canConnect
        ? Results.Ok(new { status = "ok", database = "connected" })
        : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
});

app.MapControllers();

app.Run();

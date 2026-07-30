using System.Text;
using System.Text.Json.Serialization;
using HealthcareClaims.API.Middleware;
using HealthcareClaims.Application;
using HealthcareClaims.Application.Common.Authentication;
using HealthcareClaims.Infrastructure;
using HealthcareClaims.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Application Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistence(builder.Configuration);

// JWT Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

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

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly",
        policy => policy.RequireRole("Admin"));

    options.AddPolicy("ClaimsProcessorOnly",
        policy => policy.RequireRole("ClaimsProcessor"));

    options.AddPolicy("MedicalReviewerOnly",
        policy => policy.RequireRole("MedicalReviewer"));

    options.AddPolicy("ProviderStaffOnly",
        policy => policy.RequireRole("ProviderStaff"));

    options.AddPolicy("PatientReadAccess",
        policy => policy.RequireRole(
            "Admin",
            "ClaimsProcessor",
            "MedicalReviewer",
            "ProviderStaff"));

    options.AddPolicy("PatientWriteAccess",
        policy => policy.RequireRole(
            "Admin",
            "ProviderStaff"));

    options.AddPolicy("ProviderReadAccess",
    policy => policy.RequireRole(
        "Admin",
        "ClaimsProcessor",
        "MedicalReviewer",
        "ProviderStaff"));

    options.AddPolicy("ProviderWriteAccess",
        policy => policy.RequireRole(
            "Admin",
            "ProviderStaff"));

    options.AddPolicy("InsurancePolicyReadAccess",
    policy => policy.RequireRole(
        "Admin",
        "ClaimsProcessor",
        "MedicalReviewer",
        "ProviderStaff"));

    options.AddPolicy("InsurancePolicyWriteAccess",
        policy => policy.RequireRole(
            "Admin",
            "ProviderStaff"));
});

var app = builder.Build();

// Global Exception Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
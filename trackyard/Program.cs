
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sprint1CSharp.Data;
using System.Reflection;
using Sprint1CSharp.Swagger;


var builder = WebApplication.CreateBuilder(args);

// JSON options
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.WriteIndented = true;
});

// API Versioning services
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sprint2CSharp",
        Version = "v1",
        Description = "API RESTful para controle de motos em pátios para a Mottu."
    });
    // XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    c.OperationFilter<ExamplesOperationFilter>();
    // Swagger security for API Key
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Header X-API-KEY",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme{ Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "ApiKey" } }, new List<string>() }
    });
});
if (!builder.Environment.IsEnvironment("Testing"))
{
// Connection string (env first)
string? conn =
    Environment.GetEnvironmentVariable("ORACLE_CONNECTION")
    ?? Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__ORACLECONNECTION")
    ?? builder.Configuration.GetConnectionString("OracleConnection");

if (string.IsNullOrWhiteSpace(conn) || conn.Contains("UseEnvironmentVariable", StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException("Defina a variável de ambiente ORACLE_CONNECTION com a sua connection string do Oracle.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseOracle(conn);
});
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("testing"));
}

// Health checks (DB)
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("database");

// API Key authentication/authorization
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, Sprint1CSharp.Security.ApiKeyAuthenticationHandler>("ApiKey", _ => { });
builder.Services.AddAuthorization();

// ML.NET prediction service
builder.Services.AddSingleton<Sprint1CSharp.Services.Prediction.IPredictionService, Sprint1CSharp.Services.Prediction.PredictionService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }
    catch
    {
        
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health endpoint
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }

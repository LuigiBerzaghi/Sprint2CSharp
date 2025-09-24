
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
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();

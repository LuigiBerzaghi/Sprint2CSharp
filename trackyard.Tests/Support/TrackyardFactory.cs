using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Sprint1CSharp.Data;
using Sprint1CSharp.Models;

namespace Trackyard.Tests.Support;

public class TrackyardFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var dict = new Dictionary<string,string?>
            {
                ["ApiKey"] = "test-key"
            };
            cfg.AddInMemoryCollection(dict!);
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("tests"));

            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            if (!db.Patios.Any())
            {
                db.Patios.Add(new Patio { Nome = "Pátio Central", Endereco = "Rua A, 100" });
                db.SaveChanges();
            }
        });

        return base.CreateHost(builder);
    }
}

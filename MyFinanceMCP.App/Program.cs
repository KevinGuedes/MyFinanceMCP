using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFinanceMCP.App.Data;
using MyFinanceMCP.App.Interfaces;
using MyFinanceMCP.App.MCP.Tools;
using MyFinanceMCP.App.Services;

var builder = Host.CreateEmptyApplicationBuilder(null);

var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
var myAppFolder = Path.Combine(appData, "MyFinanceMCP");
Directory.CreateDirectory(myAppFolder);
var dbPath = Path.Combine(myAppFolder, "myfinancemcp.db");
var connectionString = $"Data Source={dbPath}";

builder.Services
    .AddDbContext<MyFinanceDbContext>(options => options.UseSqlite(connectionString));

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<TransferTools>();

builder.Services
    .AddSingleton<ITransferService, TransferService>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyFinanceDbContext>();
    db.Database.EnsureCreated();
    var pendingMigrations = db.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        db.Database.Migrate();
    }
}

await host.RunAsync();

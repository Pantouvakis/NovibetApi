using Microsoft.EntityFrameworkCore;
using NovibetApi.Models;
using Microsoft.Extensions.Caching.Memory;
using NovibetApi.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();
//builder.Logging.SetMinimumLevel(LogLevel.Information);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(connectionString)
       .EnableSensitiveDataLogging()
       .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddScoped<IpInfoService>();
builder.Services.AddHostedService<IpUpdateService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

}

app.Run();

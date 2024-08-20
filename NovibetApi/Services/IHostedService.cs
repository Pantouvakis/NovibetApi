using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using NovibetApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class IpUpdateService : IHostedService, IDisposable
{
    private readonly IServiceProvider _services;
    private Timer _timer;

    public IpUpdateService(IServiceProvider services)
    {
        _services = services;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Schedule the timer to run every hour
        _timer = new Timer(UpdateIpInformation, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private async void UpdateIpInformation(object state)
    {
        using (var scope = _services.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<IpUpdateService>>();
            var ipInfoService = scope.ServiceProvider.GetRequiredService<IpInfoService>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                // Fetch all IPs from the database in batches of 100
                var batchSize = 100;
                var totalIps = await context.IPAddresses.CountAsync();
                var totalBatches = (totalIps + batchSize - 1) / batchSize;

                for (var batch = 0; batch < totalBatches; batch++)
                {
                    var ips = await context.IPAddresses
                        .Skip(batch * batchSize)
                        .Take(batchSize)
                        .ToListAsync();

                    foreach (var ip in ips)
                    {
                        var updatedIpInfo = await ipInfoService.GetIpInfoAsync(ip.IP);

                        // Check if there is a change in the information
                        if (updatedIpInfo != null && (updatedIpInfo.CountryId != ip.CountryId))
                        {
                            // Update the existing IP information
                            ip.CountryId = updatedIpInfo.CountryId;
                            ip.UpdatedAt = DateTime.UtcNow;
                            context.IPAddresses.Update(ip);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating IP information.");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

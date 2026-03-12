using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimMilletVekiliBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimMilletVekiliBackgroundService> _logger;

        public SecimMilletVekiliBackgroundService(IServiceProvider serviceProvider, ILogger<SecimMilletVekiliBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Milletvekili Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimMilletVekiliSayilari.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var response = await client.GetAsync("https://acikveri.ysk.gov.tr/api/getSecilecekAdaylar?secimId=" + secim.SecimId + "&secimTuru=" + secim.SecimTuru);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseStr = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(responseStr))
                                {
                                    try
                                    {
                                        List<SecimMilletVekiliSayisi> result = JsonConvert.DeserializeObject<List<SecimMilletVekiliSayisi>>(responseStr);
                                        foreach (var res in result)
                                        {
                                            res.SecimId = secim.Id;
                                        }

                                        await db.AddRangeAsync(result);
                                        await db.SaveChangesAsync();

                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                    }

                }
            }
            _logger.LogInformation("Secim Milletvekili Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

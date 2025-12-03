using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimIlBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimIlBackgroundService> _logger;

        public SecimIlBackgroundService(IServiceProvider serviceProvider, ILogger<SecimIlBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim İl Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimIller.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getIlList?secimId=" + secim.SecimIDAsil + "&secimTuru=" + secim.SecimTuru + "&sandikTuru=-1&yurtIciDisi=1");

                            if (response.IsSuccessStatusCode)
                            {
                                var responseStr = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(responseStr))
                                {
                                    try
                                    {
                                        List<SecimIl> result = JsonConvert.DeserializeObject<List<SecimIl>>(responseStr);
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
            _logger.LogInformation("Secim İl Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

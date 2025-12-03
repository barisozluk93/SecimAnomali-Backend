using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimIlceBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimIlceBackgroundService> _logger;

        public SecimIlceBackgroundService(IServiceProvider serviceProvider, ILogger<SecimIlceBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim İlçe Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();
                if (!await db.SecimIlceler.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var secimIller = await db.SecimIller.Where(x => x.SecimId == secim.Id).ToListAsync();

                            foreach (var secimIl in secimIller)
                            {
                                var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getIlceList?secimId=" + secim.SecimIDAsil + "&secimTuru=" + secim.SecimTuru + "&ilId=" + secimIl.il_ID + "&secimCevresiId=" + secimIl.secim_CEVRESI_ID + "&sandikTuru=-1&yurtIciDisi=1");

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseStr = await response.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(responseStr))
                                    {
                                        try
                                        {
                                            List<SecimIlce> result = JsonConvert.DeserializeObject<List<SecimIlce>>(responseStr);
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
                        }

                        transaction.Commit();
                    }
                }
            }

            _logger.LogInformation("Secim İlçe Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

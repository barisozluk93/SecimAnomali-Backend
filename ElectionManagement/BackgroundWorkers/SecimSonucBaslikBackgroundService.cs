using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimSonucBaslikBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimSonucBaslikBackgroundService> _logger;

        public SecimSonucBaslikBackgroundService(IServiceProvider serviceProvider, ILogger<SecimSonucBaslikBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Sonuç Başlık Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimSonucBasliklar.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getSandikSecimSonucBaslikList?secimId=" + secim.SecimIDAsil + "&secimCevresiId=&ilId=&bagimsiz=1&secimTuru=" + secim.SecimTuru + "&yurtIciDisi=3");

                            if (response.IsSuccessStatusCode)
                            {
                                var responseStr = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(responseStr))
                                {
                                    try
                                    {
                                        List<SecimSonucBaslik> result = JsonConvert.DeserializeObject<List<SecimSonucBaslik>>(responseStr);
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
            _logger.LogInformation("Secim Sonuç Başlık Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

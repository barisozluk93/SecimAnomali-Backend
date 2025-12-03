using ElectionManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimGenelSonucBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimGenelSonucBackgroundService> _logger;

        public SecimGenelSonucBackgroundService(IServiceProvider serviceProvider, ILogger<SecimGenelSonucBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Genel Sonuç Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimGenelSonuclar.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getSecimSonucList?secimId=" + secim.SecimIDAsil + "&secimTuru=" + secim.SecimTuru +"&ilId=&ilceId=&beldeId=&birimId=&muhtarlikId=&cezaeviId=&sandikTuru=&sandikNoIlk=&sandikNoSon=&ulkeId=&disTemsilcilikId=&gumrukId=&yurtIciDisi=1&sandikRumuzIlk=&sandikRumuzSon=&secimCevresiId=&sandikId=");

                            if (response.IsSuccessStatusCode)
                            {
                                var responseStr = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(responseStr))
                                {
                                    try
                                    {
                                        List<SecimGenelSonuc> result = JsonConvert.DeserializeObject<List<SecimGenelSonuc>>(responseStr);
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
            _logger.LogInformation("Secim Genel Sonuç Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace SecimAnomaliDataSeeder.BackgroundWorkers
{
    public class SecimSonucIlBaslikBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimSonucIlBaslikBackgroundService> _logger;

        public SecimSonucIlBaslikBackgroundService(IServiceProvider serviceProvider, ILogger<SecimSonucIlBaslikBackgroundService> logger)
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
                            var secimIller = await db.SecimIller.Where(x => x.SecimId == secim.Id).ToListAsync();

                            foreach (var secimIl in secimIller)
                            {
                                var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getSandikSecimSonucBaslikList?secimId=" + secim.SecimIDAsil + "&secimCevresiId=" + secimIl.secim_CEVRESI_ID + "&ilId=" + secimIl.il_ID + "&bagimsiz=1&secimTuru=" + secim.SecimTuru + "&yurtIciDisi=1");

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
                                                res.IlId = secimIl.il_ID;
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
            _logger.LogInformation("Secim Sonuç Başlık Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimMahalleBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimMahalleBackgroundService> _logger;

        public SecimMahalleBackgroundService(IServiceProvider serviceProvider, ILogger<SecimMahalleBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Mahalle Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();
                if (!await db.SecimMahalleler.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var secimIlceler = await db.SecimIlceler.Where(x => x.SecimId == secim.Id).ToListAsync();

                            foreach (var secimIlce in secimIlceler)
                            {
                                var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getMuhtarlikList?secimId=" + secim.SecimIDAsil + "&secimTuru=" + secim.SecimTuru + "&ilceId=" + secimIlce.ilce_ID + "&beldeId=" + secimIlce.belde_ID + "&birimId=" + secimIlce.birim_ID + "&secimCevresiId=" + secimIlce.secim_CEVRESI_ID + "&sandikTuru=-1&yurtIciDisi=1");

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseStr = await response.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(responseStr))
                                    {
                                        try
                                        {
                                            List<SecimMahalle> result = JsonConvert.DeserializeObject<List<SecimMahalle>>(responseStr);
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

            _logger.LogInformation("Secim Mahalle Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimSonucBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimSonucBackgroundService> _logger;

        public SecimSonucBackgroundService(IServiceProvider serviceProvider, ILogger<SecimSonucBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Sonuç Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimSonuclar.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();
                        foreach (var secim in secimler)
                        {
                            var secimMahalleler = await db.SecimMahalleler.Where(x => x.SecimId == secim.Id).ToListAsync();

                            foreach (var secimMahalle in secimMahalleler)
                            {
                                var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getSecimSandikSonucList?secimId=" + secim.SecimIDAsil + "&secimTuru=" + secim.SecimTuru + "&ilId=" + secimMahalle.il_ID + "&ilceId=" + secimMahalle.ilce_ID + "&beldeId=" + secimMahalle.belde_ID + "&birimId=" + secimMahalle.birim_ID + "&muhtarlikId=" + secimMahalle.muhtarlik_ID + "&cezaeviId=" + secimMahalle.cezaevi_ID + "&sandikTuru=&sandikNoIlk=" + secimMahalle.min_SANDIK_NO + "&sandikNoSon=" + secimMahalle.max_SANDIK_NO + "&ulkeId=&disTemsilcilikId=&gumrukId=&yurtIciDisi=1&sandikRumuzIlk=&sandikRumuzSon=&secimCevresiId=" + secimMahalle.secim_CEVRESI_ID + "&sandikId=&sorguTuru=2");

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseStr = await response.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(responseStr))
                                    {
                                        try
                                        {
                                            List<SecimSonuc> result = JsonConvert.DeserializeObject<List<SecimSonuc>>(responseStr);
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
            _logger.LogInformation("Secim Sonuç Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

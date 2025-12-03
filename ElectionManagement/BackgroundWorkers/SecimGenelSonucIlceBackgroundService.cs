using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimGenelSonucIlceBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimGenelSonucIlceBackgroundService> _logger;

        public SecimGenelSonucIlceBackgroundService(IServiceProvider serviceProvider, ILogger<SecimGenelSonucIlceBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Genel Sonuç İlçe Background Worker başlatıldı.");

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
                            var secimIlceler = await db.SecimIlceler.Where(x => x.SecimId == secim.Id).ToListAsync();
                        
                            foreach (var secimIlce in secimIlceler)
                            {
                                var response = await client.GetAsync("https://sonuc.ysk.gov.tr/api/getSecimSonucList?secimId=" + secim.SecimIDAsil + "&secimTuru=" + secim.SecimTuru + "&ilId=" + secimIlce.il_ID + "&ilceId=" + secimIlce.ilce_ID + "&beldeId=" + secimIlce.belde_ID + "&birimId=" + secimIlce.birim_ID + "&muhtarlikId=&cezaeviId=&sandikTuru=&sandikNoIlk=&sandikNoSon=&ulkeId=&disTemsilcilikId=&gumrukId=&yurtIciDisi=1&sandikRumuzIlk=&sandikRumuzSon=&secimCevresiId=" + secimIlce.secim_CEVRESI_ID + "&sandikId=");

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
                                                res.IlId = secimIlce.il_ID;
                                                res.IlceId = secimIlce.ilce_ID;
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
            _logger.LogInformation("Secim Genel Sonuç İlçe Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimGumrukSonuclariBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimGumrukSonuclariBackgroundService> _logger;

        public SecimGumrukSonuclariBackgroundService(IServiceProvider serviceProvider, ILogger<SecimGumrukSonuclariBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Gümrük Sonuçları Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimGumrukSonuclari.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();
                        foreach (var secim in secimler)
                        {
                            var response = await client.GetAsync("https://acikveri.ysk.gov.tr/api/getSecimSandikSonucList?secimId=" + secim.SecimId + "&secimTuru=" + secim.SecimTuru + "&ilId=&ilceId=&beldeId=&birimId=&muhtarlikId=&cezaeviId=&sandikTuru=&sandikNoIlk=&sandikNoSon=&ulkeId=&disTemsilcilikId=&gumrukId=-1&yurtIciDisi=2&sandikRumuzIlk=&sandikRumuzSon=&secimCevresiId=&sandikId=&sorguTuru=2");
                            if (response.IsSuccessStatusCode)
                            {
                                var responseStr = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(responseStr))
                                {
                                    try
                                    {
                                        List<SecimGumrukSonuc> result = JsonConvert.DeserializeObject<List<SecimGumrukSonuc>>(responseStr);
                                        foreach (var res in result)
                                        {
                                            if (res.gumruk_SANDIK_TARIHI.HasValue) {
                                                var localTime = DateTime.ParseExact(
                                                    res.gumruk_SANDIK_TARIHI.ToString(),
                                                    "d.M.yyyy HH:mm:ss",
                                                    System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));

                                                var utcTime = DateTime.SpecifyKind(localTime, DateTimeKind.Local).ToUniversalTime();

                                                res.gumruk_SANDIK_TARIHI = utcTime;
                                            }

                                            if (res.son_ISLEM_TARIHI.HasValue)
                                            {
                                                var localTime = DateTime.ParseExact(
                                                    res.son_ISLEM_TARIHI.ToString(),
                                                    "d.M.yyyy HH:mm:ss",
                                                    System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));

                                                var utcTime = DateTime.SpecifyKind(localTime, DateTimeKind.Local).ToUniversalTime();

                                                res.son_ISLEM_TARIHI = utcTime;
                                            }
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
            _logger.LogInformation("Secim Gümrük Sonuçları Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

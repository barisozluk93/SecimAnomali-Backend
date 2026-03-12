using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimGumrukListeleriBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimGumrukListeleriBackgroundService> _logger;

        public SecimGumrukListeleriBackgroundService(IServiceProvider serviceProvider, ILogger<SecimGumrukListeleriBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Gümrük Listeleri Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimGumrukListeleri.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();
                        foreach (var secim in secimler)
                        {
                            var response = await client.GetAsync("https://acikveri.ysk.gov.tr/api/getGumrukList?secimId=" + secim.SecimId);
                            if (response.IsSuccessStatusCode)
                            {
                                var responseStr = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(responseStr))
                                {
                                    try
                                    {
                                        List<SecimGumrukListesi> result = JsonConvert.DeserializeObject<List<SecimGumrukListesi>>(responseStr);
                                        foreach (var res in result)
                                        {
                                            if (res.oy_VERME_BASLANGIC_TARIHI.HasValue)
                                            {
                                                var localTime = DateTime.ParseExact(
                                                    res.oy_VERME_BASLANGIC_TARIHI.ToString(),
                                                    "d.M.yyyy HH:mm:ss",
                                                    System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));

                                                var utcTime = DateTime.SpecifyKind(localTime, DateTimeKind.Local).ToUniversalTime();

                                                res.oy_VERME_BASLANGIC_TARIHI = utcTime;
                                            }

                                            if (res.oy_VERME_BITIS_TARIHI.HasValue)
                                            {
                                                var localTime = DateTime.ParseExact(
                                                    res.oy_VERME_BITIS_TARIHI.ToString(),
                                                    "d.M.yyyy HH:mm:ss",
                                                    System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));

                                                var utcTime = DateTime.SpecifyKind(localTime, DateTimeKind.Local).ToUniversalTime();

                                                res.oy_VERME_BITIS_TARIHI = utcTime;
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
            _logger.LogInformation("Secim Gümrük Listeleri Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

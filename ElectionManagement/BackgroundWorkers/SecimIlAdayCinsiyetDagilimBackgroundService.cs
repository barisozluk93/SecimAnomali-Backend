using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimIlAdayCinsiyetDagilimBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimIlAdayCinsiyetDagilimBackgroundService> _logger;

        public SecimIlAdayCinsiyetDagilimBackgroundService(IServiceProvider serviceProvider, ILogger<SecimIlAdayCinsiyetDagilimBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim İl Aday Cinsiyet Dağılımı Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();
                if (!await db.SecimIlAdayCinsiyetDagilimlari.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var secimIller = await db.SecimIller.Where(x => x.SecimId == secim.Id).ToListAsync();

                            foreach (var secimIl in secimIller)
                            {
                                var response = await client.GetAsync("https://acikveri.ysk.gov.tr/api/getSecimTurlerineGoreAdaylarinCinsiyetDagilimiIl?secimId=" + secim.SecimId + "&secimTuru=" + secim.SecimTuru  + "&ilId=" + secimIl.il_ID);

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseStr = await response.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(responseStr))
                                    {
                                        try
                                        {
                                            List<SecimIlAdayCinsiyetDagilimi> result = JsonConvert.DeserializeObject<List<SecimIlAdayCinsiyetDagilimi>>(responseStr);
                                            foreach (var res in result)
                                            {
                                                res.il_ADI = secimIl.il_ADI;
                                                res.il_ID = secimIl.il_ID;
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

            _logger.LogInformation("Secim İl Aday Cinsiyet Dağılımı Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

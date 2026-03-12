using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimIlceSecmenCinsiyetDagilimBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimIlceSecmenCinsiyetDagilimBackgroundService> _logger;

        public SecimIlceSecmenCinsiyetDagilimBackgroundService(IServiceProvider serviceProvider, ILogger<SecimIlceSecmenCinsiyetDagilimBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim İlçe Seçmen Cinsiyet Dağılımı Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();
                if (!await db.SecimIlceSecmenCinsiyetDagilimlari.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var secimIller = await db.SecimIller.Where(x => x.SecimId == secim.Id).ToListAsync();

                            foreach (var secimIl in secimIller)
                            {
                                var response = await client.GetAsync("https://acikveri.ysk.gov.tr/api/getKadinErkekCinsiyetOraniIlGroupIlce?secimId=" + +secim.SecimId + "&secimTuru=" + secim.SecimTuru + "&ilId=" + secimIl.il_ID);

                                if (response.IsSuccessStatusCode)
                                {
                                    var responseStr = await response.Content.ReadAsStringAsync();

                                    if (!string.IsNullOrEmpty(responseStr))
                                    {
                                        try
                                        {
                                            List<SecimIlceSecmenCinsiyetDagilimi> result = JsonConvert.DeserializeObject<List<SecimIlceSecmenCinsiyetDagilimi>>(responseStr);
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

            _logger.LogInformation("Secim İlçe Seçmen Cinsiyet Dağılımı Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;

namespace ElectionManagement.BackgroundWorkers
{
    public class SecimYurtdisiSecmenYasveCinsiyetDagilimiBackgroundService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SecimYurtdisiSecmenYasveCinsiyetDagilimiBackgroundService> _logger;

        public SecimYurtdisiSecmenYasveCinsiyetDagilimiBackgroundService(IServiceProvider serviceProvider, ILogger<SecimYurtdisiSecmenYasveCinsiyetDagilimiBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Secim Yurtdisi Seçmen Cinsiyet ve Yaş Dağılımı Background Worker başlatıldı.");

            var handler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
            };
            HttpClient client = new HttpClient(handler);

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ElectionManagementDbContext>();

                if (!await db.SecimYurtdisiSecmenYasveCinsiyetDagilimlari.AnyAsync())
                {
                    using (var transaction = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var secimler = await db.Secimler.ToListAsync();

                        foreach (var secim in secimler)
                        {
                            var response = await client.GetAsync("https://acikveri.ysk.gov.tr/api/getYurtdisiYasGrubuCinsiyetDagilim?secimId=" + secim.SecimId);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseStr = await response.Content.ReadAsStringAsync();

                                if (!string.IsNullOrEmpty(responseStr))
                                {
                                    try
                                    {
                                        List<SecimYurtdisiSecmenYasveCinsiyetDagilimi> result = JsonConvert.DeserializeObject<List<SecimYurtdisiSecmenYasveCinsiyetDagilimi>>(responseStr);
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
            _logger.LogInformation("Secim Yurtdisi Seçmen Cinsiyet ve Yaş Dağılımı Background worker tamamlandı.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    }
}

using ElectionManagement.DbContexts;
using ElectionManagement.Entity;
using ElectionManagement.Interfaces;
using ElectionManagement.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElectionManagement.Services
{
    public class ElectionService : IElectionService
    {
        private readonly ElectionManagementDbContext _dbContext;

        public ElectionService(ElectionManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<PagingResult<PagedList<SecimSonuc>>>> Paginate(PagingParameter pagingParameter)
        {
            var result = new Result<PagingResult<PagedList<SecimSonuc>>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var queryable = _dbContext.SecimSonuclar
                        .Where(x => x.SecimId == pagingParameter.ElectionId);

                    if (pagingParameter.CityId > 0 && pagingParameter.DistrictId == 0 && pagingParameter.NeighborhoodId == 0)
                    {
                        queryable = queryable.Where(x => x.il_ID == pagingParameter.CityId)
                            .GroupBy(x => new { x.il_ID, x.ilce_ID })
                            .Select(g => new SecimSonuc
                            {
                                il_ID = g.Key.il_ID,
                                ilce_ID = g.Key.ilce_ID,
                                il_ADI = _dbContext.SecimIller.Where(x => x.il_ID == g.Key.il_ID).Select(s => s.il_ADI).FirstOrDefault(),
                                ilce_ADI = _dbContext.SecimIlceler.Where(x => x.ilce_ID == g.Key.ilce_ID).Select(s => s.ilce_ADI).FirstOrDefault(),

                                //Bağımsızlar
                                bagimsiz1_ALDIGI_OY = g.Sum(x => x.bagimsiz1_ALDIGI_OY),
                                bagimsiz2_ALDIGI_OY = g.Sum(x => x.bagimsiz2_ALDIGI_OY),
                                bagimsiz3_ALDIGI_OY = g.Sum(x => x.bagimsiz3_ALDIGI_OY),
                                bagimsiz4_ALDIGI_OY = g.Sum(x => x.bagimsiz4_ALDIGI_OY),
                                bagimsiz5_ALDIGI_OY = g.Sum(x => x.bagimsiz5_ALDIGI_OY),
                                bagimsiz6_ALDIGI_OY = g.Sum(x => x.bagimsiz6_ALDIGI_OY),
                                bagimsiz7_ALDIGI_OY = g.Sum(x => x.bagimsiz7_ALDIGI_OY),
                                bagimsiz8_ALDIGI_OY = g.Sum(x => x.bagimsiz8_ALDIGI_OY),
                                bagimsiz9_ALDIGI_OY = g.Sum(x => x.bagimsiz9_ALDIGI_OY),
                                bagimsiz10_ALDIGI_OY = g.Sum(x => x.bagimsiz10_ALDIGI_OY),
                                bagimsiz11_ALDIGI_OY = g.Sum(x => x.bagimsiz11_ALDIGI_OY),
                                bagimsiz12_ALDIGI_OY = g.Sum(x => x.bagimsiz12_ALDIGI_OY),
                                bagimsiz13_ALDIGI_OY = g.Sum(x => x.bagimsiz13_ALDIGI_OY),
                                bagimsiz14_ALDIGI_OY = g.Sum(x => x.bagimsiz14_ALDIGI_OY),
                                bagimsiz15_ALDIGI_OY = g.Sum(x => x.bagimsiz15_ALDIGI_OY),
                                bagimsiz16_ALDIGI_OY = g.Sum(x => x.bagimsiz16_ALDIGI_OY),
                                bagimsiz17_ALDIGI_OY = g.Sum(x => x.bagimsiz17_ALDIGI_OY),
                                bagimsiz18_ALDIGI_OY = g.Sum(x => x.bagimsiz18_ALDIGI_OY),
                                bagimsiz19_ALDIGI_OY = g.Sum(x => x.bagimsiz19_ALDIGI_OY),
                                bagimsiz20_ALDIGI_OY = g.Sum(x => x.bagimsiz20_ALDIGI_OY),
                                bagimsiz21_ALDIGI_OY = g.Sum(x => x.bagimsiz21_ALDIGI_OY),
                                bagimsiz22_ALDIGI_OY = g.Sum(x => x.bagimsiz22_ALDIGI_OY),
                                bagimsiz23_ALDIGI_OY = g.Sum(x => x.bagimsiz23_ALDIGI_OY),
                                bagimsiz24_ALDIGI_OY = g.Sum(x => x.bagimsiz24_ALDIGI_OY),
                                bagimsiz25_ALDIGI_OY = g.Sum(x => x.bagimsiz25_ALDIGI_OY),
                                bagimsiz26_ALDIGI_OY = g.Sum(x => x.bagimsiz26_ALDIGI_OY),
                                bagimsiz27_ALDIGI_OY = g.Sum(x => x.bagimsiz27_ALDIGI_OY),
                                bagimsiz28_ALDIGI_OY = g.Sum(x => x.bagimsiz28_ALDIGI_OY),
                                bagimsiz29_ALDIGI_OY = g.Sum(x => x.bagimsiz29_ALDIGI_OY),
                                bagimsiz30_ALDIGI_OY = g.Sum(x => x.bagimsiz30_ALDIGI_OY),
                                bagimsiz31_ALDIGI_OY = g.Sum(x => x.bagimsiz31_ALDIGI_OY),
                                bagimsiz32_ALDIGI_OY = g.Sum(x => x.bagimsiz32_ALDIGI_OY),
                                bagimsiz33_ALDIGI_OY = g.Sum(x => x.bagimsiz33_ALDIGI_OY),
                                bagimsiz34_ALDIGI_OY = g.Sum(x => x.bagimsiz34_ALDIGI_OY),
                                bagimsiz35_ALDIGI_OY = g.Sum(x => x.bagimsiz35_ALDIGI_OY),
                                bagimsiz36_ALDIGI_OY = g.Sum(x => x.bagimsiz36_ALDIGI_OY),
                                bagimsiz37_ALDIGI_OY = g.Sum(x => x.bagimsiz37_ALDIGI_OY),
                                bagimsiz38_ALDIGI_OY = g.Sum(x => x.bagimsiz38_ALDIGI_OY),
                                bagimsiz39_ALDIGI_OY = g.Sum(x => x.bagimsiz39_ALDIGI_OY),
                                bagimsiz40_ALDIGI_OY = g.Sum(x => x.bagimsiz40_ALDIGI_OY),
                                bagimsiz41_ALDIGI_OY = g.Sum(x => x.bagimsiz41_ALDIGI_OY),
                                bagimsiz42_ALDIGI_OY = g.Sum(x => x.bagimsiz42_ALDIGI_OY),
                                bagimsiz43_ALDIGI_OY = g.Sum(x => x.bagimsiz43_ALDIGI_OY),
                                bagimsiz44_ALDIGI_OY = g.Sum(x => x.bagimsiz44_ALDIGI_OY),
                                bagimsiz45_ALDIGI_OY = g.Sum(x => x.bagimsiz45_ALDIGI_OY),
                                bagimsiz46_ALDIGI_OY = g.Sum(x => x.bagimsiz46_ALDIGI_OY),
                                bagimsiz47_ALDIGI_OY = g.Sum(x => x.bagimsiz47_ALDIGI_OY),
                                bagimsiz48_ALDIGI_OY = g.Sum(x => x.bagimsiz48_ALDIGI_OY),
                                bagimsiz49_ALDIGI_OY = g.Sum(x => x.bagimsiz49_ALDIGI_OY),
                                bagimsiz50_ALDIGI_OY = g.Sum(x => x.bagimsiz50_ALDIGI_OY),
                                bagimsiz_TOPLAM_OY = g.Sum(x => x.bagimsiz_TOPLAM_OY),

                                // Partiler 1-40
                                parti1_ALDIGI_OY = g.Sum(x => x.parti1_ALDIGI_OY),
                                parti2_ALDIGI_OY = g.Sum(x => x.parti2_ALDIGI_OY),
                                parti3_ALDIGI_OY = g.Sum(x => x.parti3_ALDIGI_OY),
                                parti4_ALDIGI_OY = g.Sum(x => x.parti4_ALDIGI_OY),
                                parti5_ALDIGI_OY = g.Sum(x => x.parti5_ALDIGI_OY),
                                parti6_ALDIGI_OY = g.Sum(x => x.parti6_ALDIGI_OY),
                                parti7_ALDIGI_OY = g.Sum(x => x.parti7_ALDIGI_OY),
                                parti8_ALDIGI_OY = g.Sum(x => x.parti8_ALDIGI_OY),
                                parti9_ALDIGI_OY = g.Sum(x => x.parti9_ALDIGI_OY),
                                parti10_ALDIGI_OY = g.Sum(x => x.parti10_ALDIGI_OY),
                                parti11_ALDIGI_OY = g.Sum(x => x.parti11_ALDIGI_OY),
                                parti12_ALDIGI_OY = g.Sum(x => x.parti12_ALDIGI_OY),
                                parti13_ALDIGI_OY = g.Sum(x => x.parti13_ALDIGI_OY),
                                parti14_ALDIGI_OY = g.Sum(x => x.parti14_ALDIGI_OY),
                                parti15_ALDIGI_OY = g.Sum(x => x.parti15_ALDIGI_OY),
                                parti16_ALDIGI_OY = g.Sum(x => x.parti16_ALDIGI_OY),
                                parti17_ALDIGI_OY = g.Sum(x => x.parti17_ALDIGI_OY),
                                parti18_ALDIGI_OY = g.Sum(x => x.parti18_ALDIGI_OY),
                                parti19_ALDIGI_OY = g.Sum(x => x.parti19_ALDIGI_OY),
                                parti20_ALDIGI_OY = g.Sum(x => x.parti20_ALDIGI_OY),
                                parti21_ALDIGI_OY = g.Sum(x => x.parti21_ALDIGI_OY),
                                parti22_ALDIGI_OY = g.Sum(x => x.parti22_ALDIGI_OY),
                                parti23_ALDIGI_OY = g.Sum(x => x.parti23_ALDIGI_OY),
                                parti24_ALDIGI_OY = g.Sum(x => x.parti24_ALDIGI_OY),
                                parti25_ALDIGI_OY = g.Sum(x => x.parti25_ALDIGI_OY),
                                parti26_ALDIGI_OY = g.Sum(x => x.parti26_ALDIGI_OY),
                                parti27_ALDIGI_OY = g.Sum(x => x.parti27_ALDIGI_OY),
                                parti28_ALDIGI_OY = g.Sum(x => x.parti28_ALDIGI_OY),
                                parti29_ALDIGI_OY = g.Sum(x => x.parti29_ALDIGI_OY),
                                parti30_ALDIGI_OY = g.Sum(x => x.parti30_ALDIGI_OY),
                                parti31_ALDIGI_OY = g.Sum(x => x.parti31_ALDIGI_OY),
                                parti32_ALDIGI_OY = g.Sum(x => x.parti32_ALDIGI_OY),
                                parti33_ALDIGI_OY = g.Sum(x => x.parti33_ALDIGI_OY),
                                parti34_ALDIGI_OY = g.Sum(x => x.parti34_ALDIGI_OY),
                                parti35_ALDIGI_OY = g.Sum(x => x.parti35_ALDIGI_OY),
                                parti36_ALDIGI_OY = g.Sum(x => x.parti36_ALDIGI_OY),
                                parti37_ALDIGI_OY = g.Sum(x => x.parti37_ALDIGI_OY),
                                parti38_ALDIGI_OY = g.Sum(x => x.parti38_ALDIGI_OY),
                                parti39_ALDIGI_OY = g.Sum(x => x.parti39_ALDIGI_OY),
                                parti40_ALDIGI_OY = g.Sum(x => x.parti40_ALDIGI_OY),

                                // İttifaklar
                                ittifak1_ALDIGI_OY = g.Sum(x => x.ittifak1_ALDIGI_OY),
                                ittifak2_ALDIGI_OY = g.Sum(x => x.ittifak2_ALDIGI_OY),
                                ittifak3_ALDIGI_OY = g.Sum(x => x.ittifak3_ALDIGI_OY),
                                ittifak4_ALDIGI_OY = g.Sum(x => x.ittifak4_ALDIGI_OY),
                                ittifak5_ALDIGI_OY = g.Sum(x => x.ittifak5_ALDIGI_OY),
                            })
                            .OrderBy(o => o.ilce_ADI);

                    }
                    else if (pagingParameter.CityId > 0 && pagingParameter.DistrictId > 0 && pagingParameter.NeighborhoodId == 0)
                    {
                        queryable = queryable
                                .Where(x => x.il_ID == pagingParameter.CityId && x.ilce_ID == pagingParameter.DistrictId)
                                .OrderBy(o => o.sandik_NO);

                    }
                    else if (pagingParameter.CityId > 0 && pagingParameter.DistrictId > 0 && pagingParameter.NeighborhoodId > 0)
                    {
                        queryable = queryable
                            .Where(x => x.il_ID == pagingParameter.CityId && x.ilce_ID == pagingParameter.DistrictId && x.muhtarlik_ID == pagingParameter.NeighborhoodId)
                            .OrderBy(o => o.sandik_NO);

                    }
                    else
                    {
                        queryable = queryable
                            .GroupBy(x => new { x.il_ID })
                            .Select(g => new SecimSonuc
                            {
                                il_ID = g.Key.il_ID,
                                il_ADI = _dbContext.SecimIller.Where(x => x.il_ID == g.Key.il_ID).Select(s => s.il_ADI).FirstOrDefault(),
                                //Bağımsızlar
                                bagimsiz1_ALDIGI_OY = g.Sum(x => x.bagimsiz1_ALDIGI_OY),
                                bagimsiz2_ALDIGI_OY = g.Sum(x => x.bagimsiz2_ALDIGI_OY),
                                bagimsiz3_ALDIGI_OY = g.Sum(x => x.bagimsiz3_ALDIGI_OY),
                                bagimsiz4_ALDIGI_OY = g.Sum(x => x.bagimsiz4_ALDIGI_OY),
                                bagimsiz5_ALDIGI_OY = g.Sum(x => x.bagimsiz5_ALDIGI_OY),
                                bagimsiz6_ALDIGI_OY = g.Sum(x => x.bagimsiz6_ALDIGI_OY),
                                bagimsiz7_ALDIGI_OY = g.Sum(x => x.bagimsiz7_ALDIGI_OY),
                                bagimsiz8_ALDIGI_OY = g.Sum(x => x.bagimsiz8_ALDIGI_OY),
                                bagimsiz9_ALDIGI_OY = g.Sum(x => x.bagimsiz9_ALDIGI_OY),
                                bagimsiz10_ALDIGI_OY = g.Sum(x => x.bagimsiz10_ALDIGI_OY),
                                bagimsiz11_ALDIGI_OY = g.Sum(x => x.bagimsiz11_ALDIGI_OY),
                                bagimsiz12_ALDIGI_OY = g.Sum(x => x.bagimsiz12_ALDIGI_OY),
                                bagimsiz13_ALDIGI_OY = g.Sum(x => x.bagimsiz13_ALDIGI_OY),
                                bagimsiz14_ALDIGI_OY = g.Sum(x => x.bagimsiz14_ALDIGI_OY),
                                bagimsiz15_ALDIGI_OY = g.Sum(x => x.bagimsiz15_ALDIGI_OY),
                                bagimsiz16_ALDIGI_OY = g.Sum(x => x.bagimsiz16_ALDIGI_OY),
                                bagimsiz17_ALDIGI_OY = g.Sum(x => x.bagimsiz17_ALDIGI_OY),
                                bagimsiz18_ALDIGI_OY = g.Sum(x => x.bagimsiz18_ALDIGI_OY),
                                bagimsiz19_ALDIGI_OY = g.Sum(x => x.bagimsiz19_ALDIGI_OY),
                                bagimsiz20_ALDIGI_OY = g.Sum(x => x.bagimsiz20_ALDIGI_OY),
                                bagimsiz21_ALDIGI_OY = g.Sum(x => x.bagimsiz21_ALDIGI_OY),
                                bagimsiz22_ALDIGI_OY = g.Sum(x => x.bagimsiz22_ALDIGI_OY),
                                bagimsiz23_ALDIGI_OY = g.Sum(x => x.bagimsiz23_ALDIGI_OY),
                                bagimsiz24_ALDIGI_OY = g.Sum(x => x.bagimsiz24_ALDIGI_OY),
                                bagimsiz25_ALDIGI_OY = g.Sum(x => x.bagimsiz25_ALDIGI_OY),
                                bagimsiz26_ALDIGI_OY = g.Sum(x => x.bagimsiz26_ALDIGI_OY),
                                bagimsiz27_ALDIGI_OY = g.Sum(x => x.bagimsiz27_ALDIGI_OY),
                                bagimsiz28_ALDIGI_OY = g.Sum(x => x.bagimsiz28_ALDIGI_OY),
                                bagimsiz29_ALDIGI_OY = g.Sum(x => x.bagimsiz29_ALDIGI_OY),
                                bagimsiz30_ALDIGI_OY = g.Sum(x => x.bagimsiz30_ALDIGI_OY),
                                bagimsiz31_ALDIGI_OY = g.Sum(x => x.bagimsiz31_ALDIGI_OY),
                                bagimsiz32_ALDIGI_OY = g.Sum(x => x.bagimsiz32_ALDIGI_OY),
                                bagimsiz33_ALDIGI_OY = g.Sum(x => x.bagimsiz33_ALDIGI_OY),
                                bagimsiz34_ALDIGI_OY = g.Sum(x => x.bagimsiz34_ALDIGI_OY),
                                bagimsiz35_ALDIGI_OY = g.Sum(x => x.bagimsiz35_ALDIGI_OY),
                                bagimsiz36_ALDIGI_OY = g.Sum(x => x.bagimsiz36_ALDIGI_OY),
                                bagimsiz37_ALDIGI_OY = g.Sum(x => x.bagimsiz37_ALDIGI_OY),
                                bagimsiz38_ALDIGI_OY = g.Sum(x => x.bagimsiz38_ALDIGI_OY),
                                bagimsiz39_ALDIGI_OY = g.Sum(x => x.bagimsiz39_ALDIGI_OY),
                                bagimsiz40_ALDIGI_OY = g.Sum(x => x.bagimsiz40_ALDIGI_OY),
                                bagimsiz41_ALDIGI_OY = g.Sum(x => x.bagimsiz41_ALDIGI_OY),
                                bagimsiz42_ALDIGI_OY = g.Sum(x => x.bagimsiz42_ALDIGI_OY),
                                bagimsiz43_ALDIGI_OY = g.Sum(x => x.bagimsiz43_ALDIGI_OY),
                                bagimsiz44_ALDIGI_OY = g.Sum(x => x.bagimsiz44_ALDIGI_OY),
                                bagimsiz45_ALDIGI_OY = g.Sum(x => x.bagimsiz45_ALDIGI_OY),
                                bagimsiz46_ALDIGI_OY = g.Sum(x => x.bagimsiz46_ALDIGI_OY),
                                bagimsiz47_ALDIGI_OY = g.Sum(x => x.bagimsiz47_ALDIGI_OY),
                                bagimsiz48_ALDIGI_OY = g.Sum(x => x.bagimsiz48_ALDIGI_OY),
                                bagimsiz49_ALDIGI_OY = g.Sum(x => x.bagimsiz49_ALDIGI_OY),
                                bagimsiz50_ALDIGI_OY = g.Sum(x => x.bagimsiz50_ALDIGI_OY),
                                bagimsiz_TOPLAM_OY = g.Sum(x => x.bagimsiz_TOPLAM_OY),

                                // Partiler 1-40
                                parti1_ALDIGI_OY = g.Sum(x => x.parti1_ALDIGI_OY),
                                parti2_ALDIGI_OY = g.Sum(x => x.parti2_ALDIGI_OY),
                                parti3_ALDIGI_OY = g.Sum(x => x.parti3_ALDIGI_OY),
                                parti4_ALDIGI_OY = g.Sum(x => x.parti4_ALDIGI_OY),
                                parti5_ALDIGI_OY = g.Sum(x => x.parti5_ALDIGI_OY),
                                parti6_ALDIGI_OY = g.Sum(x => x.parti6_ALDIGI_OY),
                                parti7_ALDIGI_OY = g.Sum(x => x.parti7_ALDIGI_OY),
                                parti8_ALDIGI_OY = g.Sum(x => x.parti8_ALDIGI_OY),
                                parti9_ALDIGI_OY = g.Sum(x => x.parti9_ALDIGI_OY),
                                parti10_ALDIGI_OY = g.Sum(x => x.parti10_ALDIGI_OY),
                                parti11_ALDIGI_OY = g.Sum(x => x.parti11_ALDIGI_OY),
                                parti12_ALDIGI_OY = g.Sum(x => x.parti12_ALDIGI_OY),
                                parti13_ALDIGI_OY = g.Sum(x => x.parti13_ALDIGI_OY),
                                parti14_ALDIGI_OY = g.Sum(x => x.parti14_ALDIGI_OY),
                                parti15_ALDIGI_OY = g.Sum(x => x.parti15_ALDIGI_OY),
                                parti16_ALDIGI_OY = g.Sum(x => x.parti16_ALDIGI_OY),
                                parti17_ALDIGI_OY = g.Sum(x => x.parti17_ALDIGI_OY),
                                parti18_ALDIGI_OY = g.Sum(x => x.parti18_ALDIGI_OY),
                                parti19_ALDIGI_OY = g.Sum(x => x.parti19_ALDIGI_OY),
                                parti20_ALDIGI_OY = g.Sum(x => x.parti20_ALDIGI_OY),
                                parti21_ALDIGI_OY = g.Sum(x => x.parti21_ALDIGI_OY),
                                parti22_ALDIGI_OY = g.Sum(x => x.parti22_ALDIGI_OY),
                                parti23_ALDIGI_OY = g.Sum(x => x.parti23_ALDIGI_OY),
                                parti24_ALDIGI_OY = g.Sum(x => x.parti24_ALDIGI_OY),
                                parti25_ALDIGI_OY = g.Sum(x => x.parti25_ALDIGI_OY),
                                parti26_ALDIGI_OY = g.Sum(x => x.parti26_ALDIGI_OY),
                                parti27_ALDIGI_OY = g.Sum(x => x.parti27_ALDIGI_OY),
                                parti28_ALDIGI_OY = g.Sum(x => x.parti28_ALDIGI_OY),
                                parti29_ALDIGI_OY = g.Sum(x => x.parti29_ALDIGI_OY),
                                parti30_ALDIGI_OY = g.Sum(x => x.parti30_ALDIGI_OY),
                                parti31_ALDIGI_OY = g.Sum(x => x.parti31_ALDIGI_OY),
                                parti32_ALDIGI_OY = g.Sum(x => x.parti32_ALDIGI_OY),
                                parti33_ALDIGI_OY = g.Sum(x => x.parti33_ALDIGI_OY),
                                parti34_ALDIGI_OY = g.Sum(x => x.parti34_ALDIGI_OY),
                                parti35_ALDIGI_OY = g.Sum(x => x.parti35_ALDIGI_OY),
                                parti36_ALDIGI_OY = g.Sum(x => x.parti36_ALDIGI_OY),
                                parti37_ALDIGI_OY = g.Sum(x => x.parti37_ALDIGI_OY),
                                parti38_ALDIGI_OY = g.Sum(x => x.parti38_ALDIGI_OY),
                                parti39_ALDIGI_OY = g.Sum(x => x.parti39_ALDIGI_OY),
                                parti40_ALDIGI_OY = g.Sum(x => x.parti40_ALDIGI_OY),

                                // İttifaklar
                                ittifak1_ALDIGI_OY = g.Sum(x => x.ittifak1_ALDIGI_OY),
                                ittifak2_ALDIGI_OY = g.Sum(x => x.ittifak2_ALDIGI_OY),
                                ittifak3_ALDIGI_OY = g.Sum(x => x.ittifak3_ALDIGI_OY),
                                ittifak4_ALDIGI_OY = g.Sum(x => x.ittifak4_ALDIGI_OY),
                                ittifak5_ALDIGI_OY = g.Sum(x => x.ittifak5_ALDIGI_OY),
                            }).OrderBy(o => o.il_ADI);
                    }


                    var pagination = PagedList<SecimSonuc>.ToPagedList(queryable, pagingParameter.PageNumber, pagingParameter.PageSize);
                    if (pagingParameter.CityId > 0 && pagingParameter.DistrictId > 0)
                    {
                        pagination.ForEach(f => f.il_ADI = _dbContext.SecimIller.Where(x => x.il_ID == f.il_ID).Select(s => s.il_ADI).FirstOrDefault());
                        pagination.ForEach(f => f.ilce_ADI = _dbContext.SecimIlceler.Where(x => x.ilce_ID == f.ilce_ID).Select(s => s.ilce_ADI).FirstOrDefault());
                        pagination.ForEach(f => f.muhtarlik_ADI = _dbContext.SecimMahalleler.Where(x => x.muhtarlik_ID == f.muhtarlik_ID).Select(s => s.muhtarlik_ADI).FirstOrDefault());
                    }

                    result.SetData(new PagingResult<PagedList<SecimSonuc>>()
                    {
                        Items = pagination,
                        TotalCount = pagination.TotalCount,
                    });
                    result.SetMessage("İşlem başarı ile gerçekleşti.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }


        public async Task<Result<List<Secim>>> GetElections()
        {
            var result = new Result<List<Secim>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var elections = await _dbContext.Secimler.OrderBy(o => o.Id).ToListAsync();
                    result.SetData(elections);
                    result.SetMessage("İşlem başarı ile gerçekleşti.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<List<TumTurkiyeKazananPartiler>>> GetTurkeyElectionPartyResult(long electionId)
        {
            var result = new Result<List<TumTurkiyeKazananPartiler>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var secimBasliklar = await _dbContext.SecimSonucBasliklar.Where(x => x.SecimId == electionId).ToListAsync();
                    var toplamGecerliOy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => s.gecerli_OY_TOPLAMI);

                    var sonuc = new List<TumTurkiyeKazananPartiler>
    {
        new TumTurkiyeKazananPartiler { Parti = "parti1_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti1_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti2_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti2_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti3_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti3_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti4_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti4_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti5_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti5_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti6_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti6_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti7_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti7_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti8_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti8_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti9_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti9_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti10_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti10_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti11_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti11_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti12_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti12_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti13_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti13_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti14_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti14_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti15_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti15_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti16_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti16_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti17_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti17_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti18_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti18_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti19_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti19_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti20_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti20_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti21_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti21_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti22_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti22_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti23_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti23_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti24_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti24_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti25_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti25_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti26_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti26_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti27_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti27_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti28_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti28_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti29_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti29_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti30_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti30_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti31_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti31_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti32_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti32_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti33_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti33_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti34_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti34_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti35_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti35_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti36_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti36_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti37_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti37_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti38_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti38_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti39_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti39_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti40_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId).SumAsync(s => (long)s.parti40_ALDIGI_OY) }
    };

                    sonuc.ForEach(p =>
                    {
                        p.LogoPath = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.LogoPath).FirstOrDefault();
                        p.Legend = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.Legend).FirstOrDefault();
                        p.Parti = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.ad).FirstOrDefault();
                        p.Oran = toplamGecerliOy == 0
                            ? 0
                            : Math.Round((double)p.Oy / toplamGecerliOy * 100, 2);
                    });

                    var data = sonuc
                        .OrderByDescending(p => p.Oy)
                        .Take(5)
                        .ToList();

                    result.SetData(data);
                    result.SetMessage("İşlem Başarı ile gerçekleştirildi.");

                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<List<TumTurkiyeKazananPartiler>>> GetTurkeyElectionPartyResultByCity(long electionId, long cityId)
        {
            var result = new Result<List<TumTurkiyeKazananPartiler>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var secimBasliklar = await _dbContext.SecimSonucBasliklar.Where(x => x.SecimId == electionId).ToListAsync();
                    var toplamGecerliOy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => s.gecerli_OY_TOPLAMI);

                    var sonuc = new List<TumTurkiyeKazananPartiler>
    {
        new TumTurkiyeKazananPartiler { Parti = "parti1_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti1_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti2_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti2_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti3_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti3_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti4_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti4_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti5_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti5_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti6_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti6_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti7_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti7_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti8_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti8_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti9_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti9_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti10_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti10_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti11_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti11_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti12_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti12_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti13_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti13_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti14_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti14_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti15_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti15_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti16_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti16_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti17_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti17_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti18_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti18_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti19_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti19_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti20_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti20_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti21_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti21_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti22_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti22_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti23_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti23_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti24_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti24_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti25_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti25_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti26_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti26_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti27_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti27_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti28_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti28_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti29_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti29_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti30_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti30_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti31_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti31_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti32_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti32_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti33_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti33_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti34_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti34_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti35_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti35_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti36_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti36_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti37_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti37_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti38_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti38_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti39_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti39_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti40_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId).SumAsync(s => (long)s.parti40_ALDIGI_OY) }
    };

                    sonuc.ForEach(p =>
                    {
                        p.LogoPath = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.LogoPath).FirstOrDefault();
                        p.Legend = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.Legend).FirstOrDefault();
                        p.Parti = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.ad).FirstOrDefault();
                        p.Oran = toplamGecerliOy == 0
                            ? 0
                            : Math.Round((double)p.Oy / toplamGecerliOy * 100, 2);
                    });

                    var data = sonuc
                        .OrderByDescending(p => p.Oy)
                        .Take(5)
                        .ToList();

                    result.SetData(data);
                    result.SetMessage("İşlem Başarı ile gerçekleştirildi.");

                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<List<TumTurkiyeKazananPartiler>>> GetTurkeyElectionPartyResultByDistrict(long electionId, long cityId, long districtId)
        {
            var result = new Result<List<TumTurkiyeKazananPartiler>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var secimBasliklar = await _dbContext.SecimSonucBasliklar.Where(x => x.SecimId == electionId).ToListAsync();
                    var toplamGecerliOy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => s.gecerli_OY_TOPLAMI);

                    var sonuc = new List<TumTurkiyeKazananPartiler>
    {
        new TumTurkiyeKazananPartiler { Parti = "parti1_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti1_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti2_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti2_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti3_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti3_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti4_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti4_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti5_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti5_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti6_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti6_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti7_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti7_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti8_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti8_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti9_ALDIGI_OY",  Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti9_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti10_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti10_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti11_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti11_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti12_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti12_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti13_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti13_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti14_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti14_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti15_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti15_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti16_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti16_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti17_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti17_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti18_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti18_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti19_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti19_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti20_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti20_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti21_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti21_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti22_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti22_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti23_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti23_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti24_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti24_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti25_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti25_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti26_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti26_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti27_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti27_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti28_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti28_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti29_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti29_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti30_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti30_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti31_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti31_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti32_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti32_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti33_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti33_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti34_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti34_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti35_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti35_ALDIGI_OY) },

        new TumTurkiyeKazananPartiler { Parti = "parti36_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti36_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti37_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti37_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti38_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti38_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti39_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti39_ALDIGI_OY) },
        new TumTurkiyeKazananPartiler { Parti = "parti40_ALDIGI_OY", Oy = await _dbContext.SecimSonuclar.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId).SumAsync(s => (long)s.parti40_ALDIGI_OY) }
    };

                    sonuc.ForEach(p =>
                    {
                        p.LogoPath = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.LogoPath).FirstOrDefault();
                        p.Legend = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.Legend).FirstOrDefault();
                        p.Parti = secimBasliklar.Where(b => b.column_NAME.ToLower() == p.Parti.ToLower()).Select(s => s.ad).FirstOrDefault();
                        p.Oran = toplamGecerliOy == 0
                            ? 0
                            : Math.Round((double)p.Oy / toplamGecerliOy * 100, 2);
                    });

                    var data = sonuc
                        .OrderByDescending(p => p.Oy)
                        .Take(5)
                        .ToList();

                    result.SetData(data);
                    result.SetMessage("İşlem Başarı ile gerçekleştirildi.");

                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<List<TumTurkiyeKazanan>>> GetElectionResultByCity(long electionId)
        {
            var result = new Result<List<TumTurkiyeKazanan>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var ilPartiOyOranlari = await _dbContext.SecimSonuclar
                            .Where(x => x.SecimId == electionId)
                            .GroupBy(s => s.il_ID)
                            .Select(g => new
                            {
                                Il = _dbContext.SecimIller.Where(x => x.il_ID == g.Key).Select(s => s.il_ADI).FirstOrDefault(),
                                IlId = g.Key,
                                p1 = g.Sum(x => x.parti1_ALDIGI_OY),
                                p2 = g.Sum(x => x.parti2_ALDIGI_OY),
                                p3 = g.Sum(x => x.parti3_ALDIGI_OY),
                                p4 = g.Sum(x => x.parti4_ALDIGI_OY),
                                p5 = g.Sum(x => x.parti5_ALDIGI_OY),
                                p6 = g.Sum(x => x.parti6_ALDIGI_OY),
                                p7 = g.Sum(x => x.parti7_ALDIGI_OY),
                                p8 = g.Sum(x => x.parti8_ALDIGI_OY),
                                p9 = g.Sum(x => x.parti9_ALDIGI_OY),
                                p10 = g.Sum(x => x.parti10_ALDIGI_OY),
                                p11 = g.Sum(x => x.parti11_ALDIGI_OY),
                                p12 = g.Sum(x => x.parti12_ALDIGI_OY),
                                p13 = g.Sum(x => x.parti13_ALDIGI_OY),
                                p14 = g.Sum(x => x.parti14_ALDIGI_OY),
                                p15 = g.Sum(x => x.parti15_ALDIGI_OY),
                                p16 = g.Sum(x => x.parti16_ALDIGI_OY),
                                p17 = g.Sum(x => x.parti17_ALDIGI_OY),
                                p18 = g.Sum(x => x.parti18_ALDIGI_OY),
                                p19 = g.Sum(x => x.parti19_ALDIGI_OY),
                                p20 = g.Sum(x => x.parti20_ALDIGI_OY),
                                p21 = g.Sum(x => x.parti21_ALDIGI_OY),
                                p22 = g.Sum(x => x.parti22_ALDIGI_OY),
                                p23 = g.Sum(x => x.parti23_ALDIGI_OY),
                                p24 = g.Sum(x => x.parti24_ALDIGI_OY),
                                p25 = g.Sum(x => x.parti25_ALDIGI_OY),
                                p26 = g.Sum(x => x.parti26_ALDIGI_OY),
                                p27 = g.Sum(x => x.parti27_ALDIGI_OY),
                                p28 = g.Sum(x => x.parti28_ALDIGI_OY),
                                p29 = g.Sum(x => x.parti29_ALDIGI_OY),
                                p30 = g.Sum(x => x.parti30_ALDIGI_OY),
                                p31 = g.Sum(x => x.parti31_ALDIGI_OY),
                                p32 = g.Sum(x => x.parti32_ALDIGI_OY),
                                p33 = g.Sum(x => x.parti33_ALDIGI_OY),
                                p34 = g.Sum(x => x.parti34_ALDIGI_OY),
                                p35 = g.Sum(x => x.parti35_ALDIGI_OY),
                                p36 = g.Sum(x => x.parti36_ALDIGI_OY),
                                p37 = g.Sum(x => x.parti37_ALDIGI_OY),
                                p38 = g.Sum(x => x.parti38_ALDIGI_OY),
                                p39 = g.Sum(x => x.parti39_ALDIGI_OY),
                                p40 = g.Sum(x => x.parti40_ALDIGI_OY)
                            })
                            .ToListAsync();

                    var secimBasliklar = await _dbContext.SecimSonucBasliklar.Where(x => x.SecimId == electionId).ToListAsync();

                    var sonuc = ilPartiOyOranlari.Select(il =>
                    {   
                        var partiOylar = new Dictionary<string, long>
                        {
                            ["parti1_ALDIGI_OY"] = il.p1,
                            ["parti2_ALDIGI_OY"] = il.p2,
                            ["parti3_ALDIGI_OY"] = il.p3,
                            ["parti4_ALDIGI_OY"] = il.p4,
                            ["parti5_ALDIGI_OY"] = il.p5,
                            ["parti6_ALDIGI_OY"] = il.p6,
                            ["parti7_ALDIGI_OY"] = il.p7,
                            ["parti8_ALDIGI_OY"] = il.p8,
                            ["parti9_ALDIGI_OY"] = il.p9,
                            ["parti10_ALDIGI_OY"] = il.p10,
                            ["parti11_ALDIGI_OY"] = il.p11,
                            ["parti12_ALDIGI_OY"] = il.p12,
                            ["parti13_ALDIGI_OY"] = il.p13,
                            ["parti14_ALDIGI_OY"] = il.p14,
                            ["parti15_ALDIGI_OY"] = il.p15,
                            ["parti16_ALDIGI_OY"] = il.p16,
                            ["parti17_ALDIGI_OY"] = il.p17,
                            ["parti18_ALDIGI_OY"] = il.p18,
                            ["parti19_ALDIGI_OY"] = il.p19,
                            ["parti20_ALDIGI_OY"] = il.p20,
                            ["parti21_ALDIGI_OY"] = il.p21,
                            ["parti22_ALDIGI_OY"] = il.p22,
                            ["parti23_ALDIGI_OY"] = il.p23,
                            ["parti24_ALDIGI_OY"] = il.p24,
                            ["parti25_ALDIGI_OY"] = il.p25,
                            ["parti26_ALDIGI_OY"] = il.p26,
                            ["parti27_ALDIGI_OY"] = il.p27,
                            ["parti28_ALDIGI_OY"] = il.p28,
                            ["parti29_ALDIGI_OY"] = il.p29,
                            ["parti30_ALDIGI_OY"] = il.p30,
                            ["parti31_ALDIGI_OY"] = il.p31,
                            ["parti32_ALDIGI_OY"] = il.p32,
                            ["parti33_ALDIGI_OY"] = il.p33,
                            ["parti34_ALDIGI_OY"] = il.p34,
                            ["parti35_ALDIGI_OY"] = il.p35,
                            ["parti36_ALDIGI_OY"] = il.p36,
                            ["parti37_ALDIGI_OY"] = il.p37,
                            ["parti38_ALDIGI_OY"] = il.p38,
                            ["parti39_ALDIGI_OY"] = il.p39,
                            ["parti40_ALDIGI_OY"] = il.p40,
                        };

                        long toplam = partiOylar.Sum(p => p.Value);

                        var oranlar = partiOylar
                            .Where(x => x.Value > 0)
                            .Select(x => new TumTurkiyeKazananPartiler
                            {
                                Parti = secimBasliklar.Where(b => b.column_NAME.ToLower() == x.Key.ToLower()).Select(s => s.ad).FirstOrDefault(),
                                Oy = x.Value,
                                Oran = Math.Round((double)x.Value * 100 / toplam, 2)
                            })
                            .OrderByDescending(x => x.Oy)
                            .ToList();

                        return new TumTurkiyeKazanan
                        {
                            Il = il.Il,
                            IlId = il.IlId,
                            Legend = secimBasliklar.Where(b => b.ad == oranlar[0].Parti).Select(s => s.Legend).FirstOrDefault(),
                            ToplamOy = toplam,
                            Partiler = oranlar
                        };
                    })
                    .ToList();

                    result.SetData(sonuc);
                    result.SetMessage("İşlem başarı ile gerçekleştirildi.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<List<IlKazanan>>> GetElectionResultByDistrict(long electionId, long cityId)
        {
            var result = new Result<List<IlKazanan>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var ilcePartiOyOranlari = await _dbContext.SecimSonuclar
                            .Where(x => x.SecimId == electionId && x.il_ID == cityId)
                            .GroupBy(s => s.ilce_ID)
                            .Select(g => new
                            {
                                Ilce = _dbContext.SecimIlceler.Where(x => x.ilce_ID == g.Key).Select(s => s.ilce_ADI).FirstOrDefault(),
                                IlceId = g.Key,
                                p1 = g.Sum(x => x.parti1_ALDIGI_OY),
                                p2 = g.Sum(x => x.parti2_ALDIGI_OY),
                                p3 = g.Sum(x => x.parti3_ALDIGI_OY),
                                p4 = g.Sum(x => x.parti4_ALDIGI_OY),
                                p5 = g.Sum(x => x.parti5_ALDIGI_OY),
                                p6 = g.Sum(x => x.parti6_ALDIGI_OY),
                                p7 = g.Sum(x => x.parti7_ALDIGI_OY),
                                p8 = g.Sum(x => x.parti8_ALDIGI_OY),
                                p9 = g.Sum(x => x.parti9_ALDIGI_OY),
                                p10 = g.Sum(x => x.parti10_ALDIGI_OY),
                                p11 = g.Sum(x => x.parti11_ALDIGI_OY),
                                p12 = g.Sum(x => x.parti12_ALDIGI_OY),
                                p13 = g.Sum(x => x.parti13_ALDIGI_OY),
                                p14 = g.Sum(x => x.parti14_ALDIGI_OY),
                                p15 = g.Sum(x => x.parti15_ALDIGI_OY),
                                p16 = g.Sum(x => x.parti16_ALDIGI_OY),
                                p17 = g.Sum(x => x.parti17_ALDIGI_OY),
                                p18 = g.Sum(x => x.parti18_ALDIGI_OY),
                                p19 = g.Sum(x => x.parti19_ALDIGI_OY),
                                p20 = g.Sum(x => x.parti20_ALDIGI_OY),
                                p21 = g.Sum(x => x.parti21_ALDIGI_OY),
                                p22 = g.Sum(x => x.parti22_ALDIGI_OY),
                                p23 = g.Sum(x => x.parti23_ALDIGI_OY),
                                p24 = g.Sum(x => x.parti24_ALDIGI_OY),
                                p25 = g.Sum(x => x.parti25_ALDIGI_OY),
                                p26 = g.Sum(x => x.parti26_ALDIGI_OY),
                                p27 = g.Sum(x => x.parti27_ALDIGI_OY),
                                p28 = g.Sum(x => x.parti28_ALDIGI_OY),
                                p29 = g.Sum(x => x.parti29_ALDIGI_OY),
                                p30 = g.Sum(x => x.parti30_ALDIGI_OY),
                                p31 = g.Sum(x => x.parti31_ALDIGI_OY),
                                p32 = g.Sum(x => x.parti32_ALDIGI_OY),
                                p33 = g.Sum(x => x.parti33_ALDIGI_OY),
                                p34 = g.Sum(x => x.parti34_ALDIGI_OY),
                                p35 = g.Sum(x => x.parti35_ALDIGI_OY),
                                p36 = g.Sum(x => x.parti36_ALDIGI_OY),
                                p37 = g.Sum(x => x.parti37_ALDIGI_OY),
                                p38 = g.Sum(x => x.parti38_ALDIGI_OY),
                                p39 = g.Sum(x => x.parti39_ALDIGI_OY),
                                p40 = g.Sum(x => x.parti40_ALDIGI_OY)
                            })
                            .ToListAsync();

                    var secimBasliklar = await _dbContext.SecimSonucBasliklar.Where(x => x.SecimId == electionId).ToListAsync();

                    var sonuc = ilcePartiOyOranlari.Select(ilce =>
                    {
                        var partiOylar = new Dictionary<string, long>
                        {
                            ["parti1_ALDIGI_OY"] = ilce.p1,
                            ["parti2_ALDIGI_OY"] = ilce.p2,
                            ["parti3_ALDIGI_OY"] = ilce.p3,
                            ["parti4_ALDIGI_OY"] = ilce.p4,
                            ["parti5_ALDIGI_OY"] = ilce.p5,
                            ["parti6_ALDIGI_OY"] = ilce.p6,
                            ["parti7_ALDIGI_OY"] = ilce.p7,
                            ["parti8_ALDIGI_OY"] = ilce.p8,
                            ["parti9_ALDIGI_OY"] = ilce.p9,
                            ["parti10_ALDIGI_OY"] = ilce.p10,
                            ["parti11_ALDIGI_OY"] = ilce.p11,
                            ["parti12_ALDIGI_OY"] = ilce.p12,
                            ["parti13_ALDIGI_OY"] = ilce.p13,
                            ["parti14_ALDIGI_OY"] = ilce.p14,
                            ["parti15_ALDIGI_OY"] = ilce.p15,
                            ["parti16_ALDIGI_OY"] = ilce.p16,
                            ["parti17_ALDIGI_OY"] = ilce.p17,
                            ["parti18_ALDIGI_OY"] = ilce.p18,
                            ["parti19_ALDIGI_OY"] = ilce.p19,
                            ["parti20_ALDIGI_OY"] = ilce.p20,
                            ["parti21_ALDIGI_OY"] = ilce.p21,
                            ["parti22_ALDIGI_OY"] = ilce.p22,
                            ["parti23_ALDIGI_OY"] = ilce.p23,
                            ["parti24_ALDIGI_OY"] = ilce.p24,
                            ["parti25_ALDIGI_OY"] = ilce.p25,
                            ["parti26_ALDIGI_OY"] = ilce.p26,
                            ["parti27_ALDIGI_OY"] = ilce.p27,
                            ["parti28_ALDIGI_OY"] = ilce.p28,
                            ["parti29_ALDIGI_OY"] = ilce.p29,
                            ["parti30_ALDIGI_OY"] = ilce.p30,
                            ["parti31_ALDIGI_OY"] = ilce.p31,
                            ["parti32_ALDIGI_OY"] = ilce.p32,
                            ["parti33_ALDIGI_OY"] = ilce.p33,
                            ["parti34_ALDIGI_OY"] = ilce.p34,
                            ["parti35_ALDIGI_OY"] = ilce.p35,
                            ["parti36_ALDIGI_OY"] = ilce.p36,
                            ["parti37_ALDIGI_OY"] = ilce.p37,
                            ["parti38_ALDIGI_OY"] = ilce.p38,
                            ["parti39_ALDIGI_OY"] = ilce.p39,
                            ["parti40_ALDIGI_OY"] = ilce.p40,
                        };

                        long toplam = partiOylar.Sum(p => p.Value);

                        var oranlar = partiOylar
                            .Where(x => x.Value > 0)
                            .Select(x => new IlKazananPartiler
                            {
                                Parti = secimBasliklar.Where(b => b.column_NAME.ToLower() == x.Key.ToLower()).Select(s => s.ad).FirstOrDefault(),
                                Oy = x.Value,
                                Oran = Math.Round((double)x.Value * 100 / toplam, 2)
                            })
                            .OrderByDescending(x => x.Oy)
                            .ToList();

                        return new IlKazanan
                        {
                            Ilce = ilce.Ilce,
                            IlceId = ilce.IlceId,
                            Legend = secimBasliklar.Where(b => b.ad == oranlar[0].Parti).Select(s => s.Legend).FirstOrDefault(),
                            ToplamOy = toplam,
                            Partiler = oranlar
                        };
                    })
                    .ToList();

                    result.SetData(sonuc);
                    result.SetMessage("İşlem başarı ile gerçekleştirildi.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<SecimGenelSonuc>> GetElectionGeneralResult(long electionId, long cityId, long districtId, long neighborhoodId)
        {
            var result = new Result<SecimGenelSonuc>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var electionResult = _dbContext.SecimGenelSonuclar.Where(x => x.SecimId == electionId);

                    if (cityId > 0 && districtId > 0 && neighborhoodId > 0)
                    {
                        electionResult = electionResult.Where(x => x.IlId == cityId && x.IlceId == districtId && x.MahalleId == neighborhoodId);
                    }
                    else if (cityId > 0 && districtId > 0 && neighborhoodId == 0)
                    {
                        electionResult = electionResult.Where(x => x.IlId == cityId && x.IlceId == districtId && x.MahalleId == null);
                    }
                    else if (cityId > 0 && districtId == 0 && neighborhoodId == 0)
                    {
                        electionResult = electionResult.Where(x => x.IlId == cityId && x.IlceId == null && x.MahalleId == null);
                    }
                    else
                    {
                        electionResult = electionResult.Where(x => x.IlId == null && x.IlceId == null && x.MahalleId == null);
                    }

                    result.SetData(await electionResult.FirstOrDefaultAsync());
                    result.SetMessage("İşlem başarı ile gerçekleşti.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<List<SecimIl>>> GetElectionCities(long electionId)
        {
            var result = new Result<List<SecimIl>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var electionCities = await _dbContext.SecimIller.Where(x => x.SecimId == electionId)
                                                        .Select(s => new SecimIl
                                                        {
                                                            il_ID = s.il_ID,
                                                            il_ADI = s.il_ADI
                                                        })
                                                        .OrderBy(o => o.il_ADI)
                                                        .ToListAsync();
                    result.SetData(electionCities);
                    result.SetMessage("İşlem başarı ile gerçekleşti.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

        public async Task<Result<List<SecimIlce>>> GetElectionDistricts(long electionId, long cityId)
        {
            var result = new Result<List<SecimIlce>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var electionDistricts = await _dbContext.SecimIlceler.Where(x => x.SecimId == electionId && x.il_ID == cityId)
                                                                        .Select(s => new SecimIlce
                                                                        {
                                                                            ilce_ID = s.ilce_ID,
                                                                            ilce_ADI = s.ilce_ADI
                                                                        })
                                                                        .OrderBy(o => o.ilce_ADI)
                                                                         .ToListAsync();
                    result.SetData(electionDistricts);
                    result.SetMessage("İşlem başarı ile gerçekleşti.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }
        public async Task<Result<List<SecimMahalle>>> GetElectionNeighborhoods(long electionId, long cityId, long districtId)
        {
            var result = new Result<List<SecimMahalle>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var electionNeighborhoods = await _dbContext.SecimMahalleler.Where(x => x.SecimId == electionId && x.il_ID == cityId && x.ilce_ID == districtId)
                                                                        .Select(s => new SecimMahalle
                                                                        {
                                                                            muhtarlik_ID = s.muhtarlik_ID,
                                                                            muhtarlik_ADI = s.muhtarlik_ADI
                                                                        })
                                                                         .OrderBy(o => o.muhtarlik_ADI)
                                                                         .ToListAsync();
                    result.SetData(electionNeighborhoods);
                    result.SetMessage("İşlem başarı ile gerçekleşti.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }
        public async Task<Result<List<SecimSonucBaslik>>> GetElectionHeaders(long electionId, long cityId)
        {
            var result = new Result<List<SecimSonucBaslik>>();

            using (var transaction = _dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var electionHeaders = _dbContext.SecimSonucBasliklar.Where(x => x.SecimId == electionId);

                    if (cityId > 0)
                    {
                        electionHeaders = electionHeaders.Where(x => x.IlId == cityId);
                    }
                    else
                    {
                        electionHeaders = electionHeaders.Where(x => x.IlId == null);
                    }

                    result.SetData(await electionHeaders.ToListAsync());
                    result.SetMessage("İşlem başarı ile gerçekleşti.");
                }
                catch (Exception ex)
                {
                    result.SetIsSuccess(false);
                    result.SetMessage(ex.Message);
                }
            }

            return result;
        }

    }
}

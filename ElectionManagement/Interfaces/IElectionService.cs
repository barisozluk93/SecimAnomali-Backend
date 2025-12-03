using Microsoft.AspNetCore.Mvc;
using ElectionManagement.Entity;
using ElectionManagement.Model;

namespace ElectionManagement.Interfaces
{
    public interface IElectionService
    {
        Task<Result<PagingResult<PagedList<SecimSonuc>>>> Paginate(PagingParameter pagingParameter);
        Task<Result<List<Secim>>> GetElections();
        Task<Result<List<SecimIl>>> GetElectionCities(long electionId);
        Task<Result<List<SecimIlce>>> GetElectionDistricts(long electionId, long cityId);
        Task<Result<List<SecimMahalle>>> GetElectionNeighborhoods(long electionId, long cityId, long districtId);
        Task<Result<List<TumTurkiyeKazanan>>> GetElectionResultByCity(long electionId);
        Task<Result<List<IlKazanan>>> GetElectionResultByDistrict(long electionId, long cityId);
        Task<Result<SecimGenelSonuc>> GetElectionGeneralResult(long electionId, long cityId, long districtId, long neighborhoodId);
        Task<Result<List<TumTurkiyeKazananPartiler>>> GetTurkeyElectionPartyResult(long electionId);
        Task<Result<List<TumTurkiyeKazananPartiler>>> GetTurkeyElectionPartyResultByCity(long electionId, long cityId);
        Task<Result<List<TumTurkiyeKazananPartiler>>> GetTurkeyElectionPartyResultByDistrict(long electionId, long cityId, long districtId);
        Task<Result<List<SecimSonucBaslik>>> GetElectionHeaders(long electionId, long cityId);
    }
}

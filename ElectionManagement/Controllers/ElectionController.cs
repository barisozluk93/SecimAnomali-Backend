using CsvHelper;
using ElectionManagement.Entity;
using ElectionManagement.Interfaces;
using ElectionManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;

namespace ElectionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectionController : ControllerBase
    {
        readonly IElectionService electionService;

        public ElectionController(IElectionService electionService)
        {
            this.electionService = electionService;
        }

        [HttpGet("Paginate")]
        [Authorize]

        public async Task<IActionResult> Paginate([FromQuery] PagingParameter pagingParameter)
        {
            var result = await electionService.Paginate(pagingParameter);
            return new OkObjectResult(result);
        }

        [HttpGet("GetElectionHeaders/{electionId}/{cityId}")]
        [Authorize]
        public async Task<IActionResult> GetElectionHeaders(long electionId, long cityId)
        {
            var result = await electionService.GetElectionHeaders(electionId, cityId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetElections")]
        [Authorize]
        public async Task<IActionResult> GetElections()
        {
            var result = await electionService.GetElections();

            return new OkObjectResult(result);
        }

        [HttpGet("GetElectionCities/{electionId}")]
        [Authorize]
        public async Task<IActionResult> GetElectionCities(long electionId)
        {
            var result = await electionService.GetElectionCities(electionId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetElectionDistricts/{electionId}/{cityId}")]
        [Authorize]
        public async Task<IActionResult> GetElectionDistricts(long electionId, long cityId)
        {
            var result = await electionService.GetElectionDistricts(electionId, cityId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetElectionNeighborhoods/{electionId}/{cityId}/{districtId}")]
        [Authorize]
        public async Task<IActionResult> GetElectionNeighborhoods(long electionId, long cityId, long districtId)
        {
            var result = await electionService.GetElectionNeighborhoods(electionId, cityId, districtId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetElectionGeneralResult/{electionId}/{cityId}/{districtId}/{neighborhoodId}")]
        [Authorize]
        public async Task<IActionResult> GetElectionGeneralResult(long electionId, long cityId, long districtId, long neighborhoodId)
        {
            var result = await electionService.GetElectionGeneralResult(electionId, cityId, districtId, neighborhoodId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetElectionResultByCity/{electionId}")]
        [Authorize]
        public async Task<IActionResult> GetElectionResultByCity(long electionId)
        {
            var result = await electionService.GetElectionResultByCity(electionId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetElectionResultByDistrict/{electionId}/{cityId}")]
        [Authorize]
        public async Task<IActionResult> GetElectionResultByDistrict(long electionId, long cityId)
        {
            var result = await electionService.GetElectionResultByDistrict(electionId, cityId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetTurkeyElectionPartyResult/{electionId}")]
        [Authorize]
        public async Task<IActionResult> GetTurkeyElectionPartyResult(long electionId)
        {
            var result = await electionService.GetTurkeyElectionPartyResult(electionId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetTurkeyElectionPartyResultByCity/{electionId}/{cityId}")]
        [Authorize]
        public async Task<IActionResult> GetTurkeyElectionPartyResultByCity(long electionId, long cityId)
        {
            var result = await electionService.GetTurkeyElectionPartyResultByCity(electionId, cityId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetTurkeyElectionPartyResultByDistrict/{electionId}/{cityId}/{districtId}")]
        [Authorize]
        public async Task<IActionResult> GetTurkeyElectionPartyResultByDistrict(long electionId, long cityId, long districtId)
        {
            var result = await electionService.GetTurkeyElectionPartyResultByDistrict(electionId, cityId, districtId);

            return new OkObjectResult(result);
        }

        [HttpGet("GetDistrictResultCSVFormat/{cityId}/{districtId}")]

        public async Task<IActionResult> GetDistrictResultCSVFormat(long cityId, long districtId)
        {
            var result = await electionService.GetDistrictResultCSVFormat(cityId, districtId);

            var stream = new MemoryStream();
            using (var writeFile = new StreamWriter(stream, new UTF8Encoding(true), leaveOpen: true))
            {

                var csv = new CsvWriter(writeFile, CultureInfo.InvariantCulture);
                csv.WriteRecords(result.GetData());
            }
            stream.Position = 0; //reset stream
            return File(stream, "application/octet-stream", "secim.csv");
        }
    }
}

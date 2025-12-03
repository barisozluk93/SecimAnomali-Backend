using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;

namespace MapManagementService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        public MapController()
        {
        }

        [HttpGet("GetIlSinirlari")]
        public async Task<JsonResult> GetIlSinirlari()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "GeoJson\\Ýl_Sýnýrý.geojson");
            using StreamReader reader = new(path);
            return new JsonResult(reader.ReadToEnd());
        }

        [HttpGet("GetIlceSinirlari")]
        public async Task<IActionResult> GetIlceSinirlari()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "GeoJson\\Ýlçe_Sýnýrý.geojson");
            return PhysicalFile(path, "application/json");
        }
    }
}
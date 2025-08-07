using Microsoft.AspNetCore.Mvc;

namespace EnergyLegacyApp.Business.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnergyAnalyticsController : ControllerBase
    {
        private readonly EnergyAnalyticsService _service;

        public EnergyAnalyticsController(EnergyAnalyticsService service)
        {
            _service = service;
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboardData()
        {
            return Ok(_service.GetDashboardData());
        }

        [HttpGet("efficiency-analysis")]
        public IActionResult GetEfficiencyAnalysis()
        {
            return Ok(_service.GetEfficiencyAnalysis());
        }

        [HttpGet("regional-analysis")]
        public IActionResult GetRegionalAnalysis()
        {
            return Ok(_service.GetRegionalAnalysis());
        }

        [HttpGet("maintenance-alerts")]
        public IActionResult GetMaintenanceAlerts()
        {
            return Ok(_service.GetMaintenanceAlerts());
        }

        [HttpGet("export-csv")]
        public IActionResult ExportDataToCsv([FromQuery] string dataType)
        {
            return Ok(_service.ExportDataToCsv(dataType));
        }
    }
}

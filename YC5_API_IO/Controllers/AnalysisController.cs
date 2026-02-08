using Microsoft.AspNetCore.Mvc;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Added for [Authorize]

namespace YC5_API_IO.Controllers
{
    [Authorize] // Added to require authentication
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisInterface _analysisService;

        public AnalysisController(IAnalysisInterface analysisService)
        {
            _analysisService = analysisService;
        }

        /// <summary>
        /// Generates and stores the latest user statistics.
        /// </summary>
        /// <returns>A list of AnalysisDto for the generated statistics.</returns>
        [HttpPost("generate-statistics")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AnalysisDto>))]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AnalysisDto>>> GenerateStatistics()
        {
            try
            {
                var statistics = await _analysisService.GenerateUserStatisticsAsync();
                return Ok(statistics);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Exports the latest user statistics to an Excel file.
        /// </summary>
        /// <returns>An Excel file containing user statistics.</returns>
        [HttpGet("export-excel")]
        [ProducesResponseType(200, Type = typeof(FileContentResult))]
        [ProducesResponseType(500)]
        public async Task<ActionResult> ExportExcel()
        {
            try
            {
                var excelBytes = await _analysisService.ExportUserStatisticsToExcelAsync();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UserStatistics.xlsx");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

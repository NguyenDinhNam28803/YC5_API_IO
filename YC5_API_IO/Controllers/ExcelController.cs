using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;
using Microsoft.AspNetCore.Authorization;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [Authorize]
        [HttpPost("import/tasks")]
        [ProducesResponseType(200, Type = typeof(ImportTasksResult))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ImportTasks(IFormFile file)
        {
            try
            {
                var result = await _excelService.ImportTasksFromExcelAsync(file);

                if (result.Errors.Any())
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logger injected into the controller)
                // For simplicity, we'll just return a 500 Internal Server Error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets[0]; // sheet đầu tiên

            var result = new List<object>();

            int rows = sheet.Dimension.Rows;

            for (int row = 2; row <= rows; row++)
            {
                var name = sheet.Cells[row, 1].Text;
                var age = sheet.Cells[row, 2].Text;

                result.Add(new
                {
                    Name = name,
                    Age = age
                });
            }

            return Ok(result);
        }

        [HttpGet("export")]
        public IActionResult ExportExcel()
        {
            // Fake data (ví dụ todo list)
            var todos = new List<(int Id, string Title, bool IsDone)>
            {
                (1, "Learn ASP.NET Core", false),
                (2, "Learn EPPlus", true),
                (3, "Build Todo API", false)
            };

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Todos");

            // Header
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Title";
            worksheet.Cells[1, 3].Value = "Is Done";

            // Data
            int row = 2;
            foreach (var todo in todos)
            {
                worksheet.Cells[row, 1].Value = todo.Id;
                worksheet.Cells[row, 2].Value = todo.Title;
                worksheet.Cells[row, 3].Value = todo.IsDone;
                row++;
            }

            // Auto fit column
            worksheet.Cells.AutoFitColumns();

            // Convert to byte[]
            var fileBytes = package.GetAsByteArray();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "TodoList.xlsx"
            );
        }
    }
}

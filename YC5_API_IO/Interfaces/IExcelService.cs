using Microsoft.AspNetCore.Http;
using YC5_API_IO.Dto;
using YC5_API_IO.Models;

namespace YC5_API_IO.Interfaces
{
    public interface IExcelService
    {
        Task<ImportTasksResult> ImportTasksFromExcelAsync(IFormFile file);
    }
}

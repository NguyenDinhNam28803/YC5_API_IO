using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; // For FileContentResult
using YC5_API_IO.Dto; // Assuming you might want to return DTOs

namespace YC5_API_IO.Interfaces
{
    public interface IAnalysisInterface
    {
        /// <summary>
        /// Gathers and stores statistical data for all users.
        /// </summary>
        /// <returns>A collection of AnalysisDto representing the generated statistics.</returns>
        Task<IEnumerable<AnalysisDto>> GenerateUserStatisticsAsync();

        /// <summary>
        /// Exports user statistics to an Excel file.
        /// </summary>
        /// <returns>A byte array representing the Excel file content.</returns>
        Task<byte[]> ExportUserStatisticsToExcelAsync();
    }
}

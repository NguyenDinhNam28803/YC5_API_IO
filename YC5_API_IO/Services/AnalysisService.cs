using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Services
{
    public class AnalysisService : IAnalysisInterface
    {
        private readonly ApplicationDbContext _context;

        public AnalysisService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnalysisDto>> GenerateUserStatisticsAsync()
        {
            var users = await _context.Users
                                    .Include(u => u.Tasks)
                                    .Include(u => u.Comments)
                                    .Include(u => u.Categories)
                                    .Include(u => u.CountDowns)
                                    .ToListAsync();

            var analysisRecords = new List<AnalysisDto>();
            var currentAnalysisDate = DateTime.UtcNow;

            foreach (var user in users)
            {
                var totalTasks = user.Tasks?.Count ?? 0;
                var completedTasks = user.Tasks?.Count(t => t.TaskStatus == Models.TaskStatus.Completed) ?? 0;
                var totalComments = user.Comments?.Count ?? 0;
                var totalCategories = user.Categories?.Count ?? 0;
                var totalCountdowns = user.CountDowns?.Count ?? 0;

                // Determine last activity date from various activities
                var lastTaskActivity = user.Tasks?.Max(t => t.UpdatedAt ?? t.CompletedAt);
                var lastCommentActivity = user.Comments?.Max(c => c.CreatedAt);
                var lastCategoryActivity = user.Categories?.Max(c => c.CreatedAt);
                var lastCountdownActivity = user.CountDowns?.Max(cd => cd.CreatedAt);

                DateTime? lastActivityDate = null;
                if (lastTaskActivity.HasValue) lastActivityDate = lastTaskActivity;
                if (lastCommentActivity.HasValue && (lastActivityDate == null || lastCommentActivity > lastActivityDate)) lastActivityDate = lastCommentActivity;
                if (lastCategoryActivity.HasValue && (lastActivityDate == null || lastCategoryActivity > lastActivityDate)) lastActivityDate = lastCategoryActivity;
                if (lastCountdownActivity.HasValue && (lastActivityDate == null || lastCountdownActivity > lastActivityDate)) lastActivityDate = lastCountdownActivity;


                var analysis = new Analysis
                {
                    UserId = user.UserId,
                    AnalysisDate = currentAnalysisDate,
                    TotalTasks = totalTasks,
                    CompletedTasks = completedTasks,
                    TotalComments = totalComments,
                    TotalCategories = totalCategories,
                    TotalCountdowns = totalCountdowns,
                    LastActivityDate = lastActivityDate
                };

                await _context.Analysis.AddAsync(analysis);

                analysisRecords.Add(new AnalysisDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName, // Assuming UserName is available and you want to expose it
                    AnalysisDate = analysis.AnalysisDate,
                    TotalTasks = analysis.TotalTasks,
                    CompletedTasks = analysis.CompletedTasks,
                    TotalComments = analysis.TotalComments,
                    TotalCategories = analysis.TotalCategories,
                    TotalCountdowns = analysis.TotalCountdowns,
                    LastActivityDate = analysis.LastActivityDate
                });
            }

            await _context.SaveChangesAsync();
            return analysisRecords;
        }

        public async Task<byte[]> ExportUserStatisticsToExcelAsync()
        {
            var latestAnalysis = await _context.Analysis
                                            .Include(a => a.User)
                                            .GroupBy(a => a.UserId)
                                            .Select(g => g.OrderByDescending(a => a.AnalysisDate).FirstOrDefault())
                                            .Where(a => a != null)
                                            .Select(a => new AnalysisDto
                                            {
                                                UserId = a.UserId,
                                                UserName = a.User != null ? a.User.UserName : "Unknown User", // Handle potential null User
                                                AnalysisDate = a.AnalysisDate,
                                                TotalTasks = a.TotalTasks,
                                                CompletedTasks = a.CompletedTasks,
                                                TotalComments = a.TotalComments,
                                                TotalCategories = a.TotalCategories,
                                                TotalCountdowns = a.TotalCountdowns,
                                                LastActivityDate = a.LastActivityDate
                                            })
                                            .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("User Statistics");

                // Add headers
                worksheet.Cells[1, 1].Value = "User ID";
                worksheet.Cells[1, 2].Value = "User Name";
                worksheet.Cells[1, 3].Value = "Analysis Date";
                worksheet.Cells[1, 4].Value = "Total Tasks";
                worksheet.Cells[1, 5].Value = "Completed Tasks";
                worksheet.Cells[1, 6].Value = "Total Comments";
                worksheet.Cells[1, 7].Value = "Total Categories";
                worksheet.Cells[1, 8].Value = "Total Countdowns";
                worksheet.Cells[1, 9].Value = "Last Activity Date";

                // Add data
                for (int i = 0; i < latestAnalysis.Count; i++)
                {
                    var record = latestAnalysis[i];
                    worksheet.Cells[i + 2, 1].Value = record.UserId;
                    worksheet.Cells[i + 2, 2].Value = record.UserName;
                    worksheet.Cells[i + 2, 3].Value = record.AnalysisDate.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[i + 2, 4].Value = record.TotalTasks;
                    worksheet.Cells[i + 2, 5].Value = record.CompletedTasks;
                    worksheet.Cells[i + 2, 6].Value = record.TotalComments;
                    worksheet.Cells[i + 2, 7].Value = record.TotalCategories;
                    worksheet.Cells[i + 2, 8].Value = record.TotalCountdowns;
                    worksheet.Cells[i + 2, 9].Value = record.LastActivityDate?.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Auto-fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return await package.GetAsByteArrayAsync();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Services
{
    public class CommentService : ICommentInterface
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(string userId, string? taskId = null)
        {
            var query = _context.Comments.Where(c => c.UserId == userId);

            if (!string.IsNullOrEmpty(taskId))
            {
                query = query.Where(c => c.TaskId == taskId);
            }

            return await query.OrderByDescending(c => c.CreateAt).ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(string userId, string commentId)
        {
            return await _context.Comments
                                 .Where(c => c.UserId == userId && c.CommentId == commentId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<Comment> CreateCommentAsync(string userId, CreateCommentDto createCommentDto)
        {
            // Verify if the Task exists and belongs to the user
            var taskExists = await _context.Tasks.AnyAsync(t => t.UserId == userId && t.TaskId == createCommentDto.TaskId);
            if (!taskExists)
            {
                throw new InvalidOperationException("Task not found or does not belong to the user.");
            }

            var comment = new Comment
            {
                UserId = userId,
                TaskId = createCommentDto.TaskId,
                CommentTitle = createCommentDto.CommentTitle,
                CommentText = createCommentDto.CommentText,
                CreateAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(string userId, string commentId, UpdateCommentDto updateCommentDto)
        {
            var comment = await _context.Comments
                                         .Where(c => c.UserId == userId && c.CommentId == commentId)
                                         .FirstOrDefaultAsync();

            if (comment == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateCommentDto.CommentTitle))
            {
                comment.CommentTitle = updateCommentDto.CommentTitle;
            }
            if (!string.IsNullOrEmpty(updateCommentDto.CommentText))
            {
                comment.CommentText = updateCommentDto.CommentText;
            }
            
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(string userId, string commentId)
        {
            var comment = await _context.Comments
                                         .Where(c => c.UserId == userId && c.CommentId == commentId)
                                         .FirstOrDefaultAsync();

            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

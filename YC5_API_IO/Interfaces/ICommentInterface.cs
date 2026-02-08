using YC5_API_IO.Dto;
using YC5_API_IO.Models;

namespace YC5_API_IO.Interfaces
{
    public interface ICommentInterface
    {
        Task<IEnumerable<Comment>> GetCommentsAsync(string userId, string? taskId = null);
        Task<Comment?> GetCommentByIdAsync(string userId, string commentId);
        Task<Comment> CreateCommentAsync(string userId, CreateCommentDto createCommentDto);
        Task<Comment?> UpdateCommentAsync(string userId, string commentId, UpdateCommentDto updateCommentDto);
        Task<bool> DeleteCommentAsync(string userId, string commentId);
    }
}

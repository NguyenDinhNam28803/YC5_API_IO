using YC5_API_IO.Models;
using YC5_API_IO.Dto;

namespace YC5_API_IO.Interfaces
{
    public interface ITagInterface
    {
        Task<IEnumerable<Tag>> GetTagsAsync(string userId);
        Task<Tag?> GetTagByIdAsync(string userId, string tagId);
        Task<Tag> CreateTagAsync(string userId, CreateTagDto createTagDto);
        Task<Tag?> UpdateTagAsync(string userId, string tagId, UpdateTagDto updateTagDto);
        Task<bool> DeleteTagAsync(string userId, string tagId);
        Task<bool> TagExistsAsync(string userId, string tagId);
    }
}

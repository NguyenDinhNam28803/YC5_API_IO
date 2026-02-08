using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Services
{
    public class TagService : ITagInterface
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync(string userId)
        {
            return await _context.Tags
                                 .Where(t => t.UserId == userId && !t.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(string userId, string tagId)
        {
            return await _context.Tags
                                 .Where(t => t.UserId == userId && t.TagId == tagId && !t.IsDeleted)
                                 .FirstOrDefaultAsync();
        }

        public async Task<Tag> CreateTagAsync(string userId, CreateTagDto createTagDto)
        {
            var tag = new Tag
            {
                UserId = userId,
                TagName = createTagDto.TagName,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> UpdateTagAsync(string userId, string tagId, UpdateTagDto updateTagDto)
        {
            var tag = await GetTagByIdAsync(userId, tagId);

            if (tag == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateTagDto.TagName))
            {
                tag.TagName = updateTagDto.TagName;
            }

            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteTagAsync(string userId, string tagId)
        {
            var tag = await GetTagByIdAsync(userId, tagId);
            if (tag == null)
            {
                return false;
            }

            tag.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TagExistsAsync(string userId, string tagId)
        {
            return await _context.Tags.AnyAsync(t => t.UserId == userId && t.TagId == tagId && !t.IsDeleted);
        }
    }
}

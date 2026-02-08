using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Services
{
    public class CategoryService : ICategoryInterface
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(string userId)
        {
            return await _context.Categories
                                 .Where(c => c.UserId == userId && !c.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(string userId, string categoryId)
        {
            return await _context.Categories
                                 .Where(c => c.UserId == userId && c.CategoryId == categoryId && !c.IsDeleted)
                                 .FirstOrDefaultAsync();
        }

        public async Task<Category> CreateCategoryAsync(string userId, CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                UserId = userId,
                CategoryName = createCategoryDto.CategoryName,
                CategoryDescription = createCategoryDto.CategoryDescription,
                Color = createCategoryDto.Color,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(string userId, string categoryId, UpdateCategoryDto updateCategoryDto)
        {
            var category = await GetCategoryByIdAsync(userId, categoryId);

            if (category == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateCategoryDto.CategoryName))
            {
                category.CategoryName = updateCategoryDto.CategoryName;
            }

            if (!string.IsNullOrEmpty(updateCategoryDto.CategoryDescription))
            {
                category.CategoryDescription = updateCategoryDto.CategoryDescription;
            }

            if (!string.IsNullOrEmpty(updateCategoryDto.Color))
            {
                category.Color = updateCategoryDto.Color;
            }

            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(string userId, string categoryId)
        {
            var category = await GetCategoryByIdAsync(userId, categoryId);
            if (category == null)
            {
                return false;
            }

            category.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CategoryExistsAsync(string userId, string categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.UserId == userId && c.CategoryId == categoryId && !c.IsDeleted);
        }
    }
}

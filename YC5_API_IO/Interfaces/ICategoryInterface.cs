using YC5_API_IO.Models;
using YC5_API_IO.Dto;

namespace YC5_API_IO.Interfaces
{
    public interface ICategoryInterface
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(string userId);
        Task<Category?> GetCategoryByIdAsync(string userId, string categoryId);
        Task<Category> CreateCategoryAsync(string userId, CreateCategoryDto createCategoryDto);
        Task<Category?> UpdateCategoryAsync(string userId, string categoryId, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteCategoryAsync(string userId, string categoryId);
        Task<bool> CategoryExistsAsync(string userId, string categoryId);
        Task<Category?> GetCategoryByNameAsync(string userId, string categoryName); // New method
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires authentication
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryInterface _categoryService;

        public CategoriesController(ICategoryInterface categoryService)
        {
            _categoryService = categoryService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            try
            {
                var userId = GetUserId();
                var categories = await _categoryService.GetCategoriesAsync(userId);
                return Ok(new
                {
                    success = true,
                    message = "Categories retrieved successfully",
                    data = categories.Select(c => new CategoryDto
                    {
                        CategoryId = c.CategoryId,
                        UserId = c.UserId,
                        CategoryName = c.CategoryName,
                        CategoryDescription = c.CategoryDescription,
                        Color = c.Color,
                        CreatedAt = c.CreatedAt,
                        IsDeleted = c.IsDeleted
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/Categories/{categoryId}
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(string categoryId)
        {
            try
            {
                var userId = GetUserId();
                var category = await _categoryService.GetCategoryByIdAsync(userId, categoryId);

                if (category == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Category not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Category retrieved successfully",
                    data = new CategoryDto
                    {
                        CategoryId = category.CategoryId,
                        UserId = category.UserId,
                        CategoryName = category.CategoryName,
                        CategoryDescription = category.CategoryDescription,
                        Color = category.Color,
                        CreatedAt = category.CreatedAt,
                        IsDeleted = category.IsDeleted
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            try
            {
                var userId = GetUserId();
                var category = await _categoryService.CreateCategoryAsync(userId, createCategoryDto);
                return Ok(new
                {
                    success = true,
                    message = "Category created successfully",
                    data = new CategoryDto
                    {
                        CategoryId = category.CategoryId,
                        UserId = category.UserId,
                        CategoryName = category.CategoryName,
                        CategoryDescription = category.CategoryDescription,
                        Color = category.Color,
                        CreatedAt = category.CreatedAt,
                        IsDeleted = category.IsDeleted
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/Categories/{categoryId}
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(string categoryId, UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var userId = GetUserId();
                var updatedCategory = await _categoryService.UpdateCategoryAsync(userId, categoryId, updateCategoryDto);

                if (updatedCategory == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Category not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Category updated successfully",
                    data = new CategoryDto
                    {
                        CategoryId = updatedCategory.CategoryId,
                        UserId = updatedCategory.UserId,
                        CategoryName = updatedCategory.CategoryName,
                        CategoryDescription = updatedCategory.CategoryDescription,
                        Color = updatedCategory.Color,
                        CreatedAt = updatedCategory.CreatedAt,
                        IsDeleted = updatedCategory.IsDeleted
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/Categories/{categoryId}
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(string categoryId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _categoryService.DeleteCategoryAsync(userId, categoryId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Category not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Category deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}

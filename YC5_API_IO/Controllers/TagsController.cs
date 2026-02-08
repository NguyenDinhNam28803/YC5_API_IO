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
    public class TagsController : ControllerBase
    {
        private readonly ITagInterface _tagService;

        public TagsController(ITagInterface tagService)
        {
            _tagService = tagService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
        {
            try
            {
                var userId = GetUserId();
                var tags = await _tagService.GetTagsAsync(userId);
                return Ok(new
                {
                    success = true,
                    message = "Tags retrieved successfully",
                    data = tags.Select(t => new TagDto
                    {
                        TagId = t.TagId,
                        UserId = t.UserId,
                        TagName = t.TagName,
                        CreatedAt = t.CreatedAt,
                        IsDeleted = t.IsDeleted
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

        // GET: api/Tags/{tagId}
        [HttpGet("{tagId}")]
        public async Task<ActionResult<TagDto>> GetTag(string tagId)
        {
            try
            {
                var userId = GetUserId();
                var tag = await _tagService.GetTagByIdAsync(userId, tagId);

                if (tag == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Tag not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Tag retrieved successfully",
                    data = new TagDto
                    {
                        TagId = tag.TagId,
                        UserId = tag.UserId,
                        TagName = tag.TagName,
                        CreatedAt = tag.CreatedAt,
                        IsDeleted = tag.IsDeleted
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

        // POST: api/Tags
        [HttpPost]
        public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto createTagDto)
        {
            try
            {
                var userId = GetUserId();
                var tag = await _tagService.CreateTagAsync(userId, createTagDto);
                return Ok(new
                {
                    success = true,
                    message = "Tag created successfully",
                    data = new TagDto
                    {
                        TagId = tag.TagId,
                        UserId = tag.UserId,
                        TagName = tag.TagName,
                        CreatedAt = tag.CreatedAt,
                        IsDeleted = tag.IsDeleted
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

        // PUT: api/Tags/{tagId}
        [HttpPut("{tagId}")]
        public async Task<IActionResult> UpdateTag(string tagId, UpdateTagDto updateTagDto)
        {
            try
            {
                var userId = GetUserId();
                var updatedTag = await _tagService.UpdateTagAsync(userId, tagId, updateTagDto);

                if (updatedTag == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Tag not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Tag updated successfully",
                    data = new TagDto
                    {
                        TagId = updatedTag.TagId,
                        UserId = updatedTag.UserId,
                        TagName = updatedTag.TagName,
                        CreatedAt = updatedTag.CreatedAt,
                        IsDeleted = updatedTag.IsDeleted
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

        // DELETE: api/Tags/{tagId}
        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteTag(string tagId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _tagService.DeleteTagAsync(userId, tagId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Tag not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Tag deleted successfully"
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

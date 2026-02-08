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
            var userId = GetUserId();
            var tags = await _tagService.GetTagsAsync(userId);
            return Ok(tags.Select(t => new TagDto
            {
                TagId = t.TagId,
                UserId = t.UserId,
                TagName = t.TagName,
                CreatedAt = t.CreatedAt,
                IsDeleted = t.IsDeleted
            }));
        }

        // GET: api/Tags/{tagId}
        [HttpGet("{tagId}")]
        public async Task<ActionResult<TagDto>> GetTag(string tagId)
        {
            var userId = GetUserId();
            var tag = await _tagService.GetTagByIdAsync(userId, tagId);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(new TagDto
            {
                TagId = tag.TagId,
                UserId = tag.UserId,
                TagName = tag.TagName,
                CreatedAt = tag.CreatedAt,
                IsDeleted = tag.IsDeleted
            });
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto createTagDto)
        {
            var userId = GetUserId();
            var tag = await _tagService.CreateTagAsync(userId, createTagDto);
            return CreatedAtAction(nameof(GetTag), new { tagId = tag.TagId }, new TagDto
            {
                TagId = tag.TagId,
                UserId = tag.UserId,
                TagName = tag.TagName,
                CreatedAt = tag.CreatedAt,
                IsDeleted = tag.IsDeleted
            });
        }

        // PUT: api/Tags/{tagId}
        [HttpPut("{tagId}")]
        public async Task<IActionResult> UpdateTag(string tagId, UpdateTagDto updateTagDto)
        {
            var userId = GetUserId();
            var updatedTag = await _tagService.UpdateTagAsync(userId, tagId, updateTagDto);

            if (updatedTag == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Tags/{tagId}
        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteTag(string tagId)
        {
            var userId = GetUserId();
            var result = await _tagService.DeleteTagAsync(userId, tagId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

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
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentInterface _commentService;

        public CommentsController(ICommentInterface commentService)
        {
            _commentService = commentService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");
        }

        // GET: api/Comments?taskId={taskId}
        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] string? taskId = null)
        {
            try
            {
                var userId = GetUserId();
                var comments = await _commentService.GetCommentsAsync(userId, taskId);
                return Ok(new
                {
                    success = true,
                    message = "Comments retrieved successfully",
                    data = comments.Select(c => new CommentDto
                    {
                        CommentId = c.CommentId,
                        TaskId = c.TaskId,
                        UserId = c.UserId,
                        CommentTitle = c.CommentTitle,
                        CommentText = c.CommentText,
                        CreateAt = c.CreateAt
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

        // GET: api/Comments/{commentId}
        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetComment(string commentId)
        {
            try
            {
                var userId = GetUserId();
                var comment = await _commentService.GetCommentByIdAsync(userId, commentId);

                if (comment == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Comment not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Comment retrieved successfully",
                    data = new CommentDto
                    {
                        CommentId = comment.CommentId,
                        TaskId = comment.TaskId,
                        UserId = comment.UserId,
                        CommentTitle = comment.CommentTitle,
                        CommentText = comment.CommentText,
                        CreateAt = comment.CreateAt
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

        // POST: api/Comments
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                var userId = GetUserId();
                var newComment = await _commentService.CreateCommentAsync(userId, createCommentDto);

                return CreatedAtAction(nameof(GetComment), new { commentId = newComment.CommentId }, new
                {
                    success = true,
                    message = "Comment created successfully",
                    data = new CommentDto
                    {
                        CommentId = newComment.CommentId,
                        TaskId = newComment.TaskId,
                        UserId = newComment.UserId,
                        CommentTitle = newComment.CommentTitle,
                        CommentText = newComment.CommentText,
                        CreateAt = newComment.CreateAt
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
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

        // PUT: api/Comments/{commentId}
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(string commentId, [FromBody] UpdateCommentDto updateCommentDto)
        {
            try
            {
                var userId = GetUserId();
                var updatedComment = await _commentService.UpdateCommentAsync(userId, commentId, updateCommentDto);

                if (updatedComment == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Comment not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Comment updated successfully",
                    data = new CommentDto
                    {
                        CommentId = updatedComment.CommentId,
                        TaskId = updatedComment.TaskId,
                        UserId = updatedComment.UserId,
                        CommentTitle = updatedComment.CommentTitle,
                        CommentText = updatedComment.CommentText,
                        CreateAt = updatedComment.CreateAt
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

        // DELETE: api/Comments/{commentId}
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _commentService.DeleteCommentAsync(userId, commentId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Comment not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Comment deleted successfully"
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

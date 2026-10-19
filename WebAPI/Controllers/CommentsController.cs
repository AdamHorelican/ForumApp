using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositaryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;

    public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo,
        IPostRepository postRepo)
    {
        this.commentRepo = commentRepo;
        this.userRepo = userRepo;
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        try
        {
            try
            {
                await userRepo.GetByIdAsync(request.UserId);
                await postRepo.GetByIdAsync(request.PostId);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }

            Comment comment = new Comment
            {
                Body = request.Body,
                UserId = request.UserId,
                PostId = request.PostId
            };

            Comment created = await commentRepo.AddAsync(comment);

            CommentDto dto = new CommentDto
            {
                Id = created.Id,
                Body = created.Body,
                UserId = created.UserId,
                PostId = created.PostId
            };

            return Created($"/Comments/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment([FromRoute] int id)
    {
        try
        {
            Comment comment = await commentRepo.GetByIdAsync(id);
            return Ok(new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId
            });
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<List<CommentDto>> GetManyComments([FromQuery] int? userId,
        [FromQuery] string? username, [FromQuery] int? postId)
    {
        try
        {
            IQueryable<Comment> query = commentRepo.GetAll();

            if (userId.HasValue)
            {
                query = query.Where(c => c.UserId == userId.Value);
            }

            if (postId.HasValue)
            {
                query = query.Where(c => c.PostId == postId.Value);
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                List<int> matchingUserIds = userRepo.GetAll()
                    .Where(u => u.Username.ToLower().Contains(username.ToLower()))
                    .Select(u => u.Id)
                    .ToList();

                query = query.Where(c => matchingUserIds.Contains(c.UserId));
            }

            List<CommentDto> dtos = query
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId
                })
                .ToList();

            return Ok(dtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> UpdateComment([FromRoute] int id,
        [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment existing = await commentRepo.GetByIdAsync(id);

            existing.Body = request.Body;
            await commentRepo.UpdateAsync(existing);

            return Ok(new CommentDto
            {
                Id = existing.Id,
                Body = existing.Body,
                UserId = existing.UserId,
                PostId = existing.PostId
            });
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
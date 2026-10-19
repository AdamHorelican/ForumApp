using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositaryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo,
        ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            try
            {
                await userRepo.GetByIdAsync(request.UserId);
            }
            catch (InvalidOperationException)
            {
                return BadRequest($"No user with id {request.UserId}.");
            }

            Post post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId
            };

            Post created = await postRepo.AddAsync(post);

            PostDto dto = new PostDto
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId
            };

            return Created($"/Posts/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSinglePost([FromRoute] int id)
    {
        try
        {
            Post post = await postRepo.GetByIdAsync(id);
            return Ok(new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
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
    public ActionResult<List<PostDto>> GetManyPosts([FromQuery] string? titleContains,
        [FromQuery] int? userId, [FromQuery] string? username)
    {
        try
        {
            IQueryable<Post> query = postRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(titleContains))
            {
                query = query.Where(p =>
                    p.Title.ToLower().Contains(titleContains.ToLower()));
            }

            if (userId.HasValue)
            {
                query = query.Where(p => p.UserId == userId.Value);
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                List<int> matchingUserIds = userRepo.GetAll()
                    .Where(u => u.Username.ToLower().Contains(username.ToLower()))
                    .Select(u => u.Id)
                    .ToList();

                query = query.Where(p => matchingUserIds.Contains(p.UserId));
            }

            List<PostDto> dtos = query
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    UserId = p.UserId
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

    [HttpGet("{id:int}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsForPost([FromRoute] int id)
    {
        try
        {
            await postRepo.GetByIdAsync(id);

            List<CommentDto> dtos = commentRepo.GetAll()
                .Where(c => c.PostId == id)
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

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostDto>> UpdatePost([FromRoute] int id,
        [FromBody] UpdatePostDto request)
    {
        try
        {
            Post existing = await postRepo.GetByIdAsync(id);

            existing.Title = request.Title;
            existing.Body = request.Body;
            await postRepo.UpdateAsync(existing);

            return Ok(new PostDto
            {
                Id = existing.Id,
                Title = existing.Title,
                Body = existing.Body,
                UserId = existing.UserId
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
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
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
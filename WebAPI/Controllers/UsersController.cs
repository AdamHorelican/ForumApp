using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositaryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        try
        {
            if (userRepo.GetAll().Any(u => u.Username == request.Username))
            {
                return BadRequest($"Username '{request.Username}' is already taken.");
            }

            User user = new User
            {
                Username = request.Username,
                Password = request.Password
            };

            User created = await userRepo.AddAsync(user);

            UserDto dto = new UserDto
            {
                Id = created.Id,
                Username = created.Username
            };

            return Created($"/Users/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingleUser([FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetByIdAsync(id);
            return Ok(new UserDto { Id = user.Id, Username = user.Username });
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
    public ActionResult<List<UserDto>> GetManyUsers([FromQuery] string? usernameContains)
    {
        try
        {
            IQueryable<User> query = userRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(usernameContains))
            {
                query = query.Where(u =>
                    u.Username.ToLower().Contains(usernameContains.ToLower()));
            }

            List<UserDto> dtos = query
                .Select(u => new UserDto { Id = u.Id, Username = u.Username })
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
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] int id,
        [FromBody] UpdateUserDto request)
    {
        try
        {
            User existing = await userRepo.GetByIdAsync(id);

            if (userRepo.GetAll().Any(u => u.Username == request.Username && u.Id != id))
            {
                return BadRequest($"Username '{request.Username}' is already taken.");
            }

            existing.Username = request.Username;
            existing.Password = request.Password;
            await userRepo.UpdateAsync(existing);

            return Ok(new UserDto { Id = existing.Id, Username = existing.Username });
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
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
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
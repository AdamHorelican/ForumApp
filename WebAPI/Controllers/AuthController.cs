using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositaryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        try
        {
            User? user = await userRepo.GetAll()
                .SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user is null || user.Password != request.Password)
            {
                return Unauthorized("Incorrect username or password.");
            }

            return Ok(new UserDto { Id = user.Id, Username = user.Username });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
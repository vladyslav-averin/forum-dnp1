using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

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
            User? user = await userRepo.GetMany().SingleOrDefaultAsync(user =>
                user.UserName.ToLower() == request.UserName.ToLower());

            if (user is null)
            {
                return Unauthorized("Invalid username or password.");
            }

            if (user.Password != request.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            UserDto dto = new()
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}

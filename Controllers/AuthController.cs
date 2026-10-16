using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public AuthController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult Register(RaceDay.APIx.Models.AppUser dto)
    {
        //Password Hash before saving
        dto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
        var user = new AppUser
        {
            Role = dto.Role,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = dto.PasswordHash
        };
        _context.Users.Add(user);
        _context.SaveChanges();
        return Created("", "User  registered");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return Unauthorized("Invalid Credentials");

        //set session state[cite : 24]
        HttpContext.Session.SetInt32("UserID", user.UserID);
        HttpContext.Session.SetString("UserRole", user.Role);
        return Ok(new { Message = "Logged in Succesfully", Role = user.Role });
    }
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set;  }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(UserManager<MyUser> userManager, TokenService tokenService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
    {
        var user = await userManager.FindByNameAsync(loginUser.Email);
        if (user == null)
        {
            return BadRequest($"User with name: {loginUser.Email} not exists!");
        }

        if (!await userManager.CheckPasswordAsync(user, loginUser.Password))
        {
            return Unauthorized("Email and password is incorrect!");
        }

        var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtToken = tokenService.GetToken(authClaims);
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            expiration = jwtToken.ValidTo
        });
    }
}

public class LoginUser
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
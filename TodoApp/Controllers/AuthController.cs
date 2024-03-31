using Microsoft.AspNetCore.Mvc;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    
    [HttpGet("add-roles")]
    public IActionResult AddRoles()
    {
        authService.AddRoles();
        return Ok();
    }
}
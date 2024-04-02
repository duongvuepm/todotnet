using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("/users")]
public class UserController(UserService userService, AuthContext authContext) : ControllerBase
{
    
    [HttpGet("add-roles")]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> AddRoles()
    {
        var response = await userService.AddRoles();
        return Ok(response);
    }
    
    [HttpPost("SetRole")]
    public ActionResult<UserResponse> MapUserRole([FromQuery] string userName, [FromQuery] string role)
    {
        return Ok(userService.SetUserRole(userName, role));
    }
    
    [HttpGet("{userId}")]
    public ActionResult<UserResponse> GetUser(string userId)
    {
        return Ok(userService.GetUser(userId));
    }
}
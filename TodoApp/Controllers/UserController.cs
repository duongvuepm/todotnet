using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("/users")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("add-roles")]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> AddRoles()
    {
        var response = await userService.AddRoles();
        return Ok(response);
    }

    [HttpPost("SetRole")]
    public async Task<ActionResult<UserResponse>> MapUserRole([FromQuery] string userName, [FromQuery] string role)
    {
        UserResponse user = await userService.SetUserRole(userName, role);
        return Ok(user);
    }

    [HttpGet("{userId}")]
    public ActionResult<UserResponse> GetUser(string userId)
    {
        return Ok(userService.GetUser(userId));
    }
}
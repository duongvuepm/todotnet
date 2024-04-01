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

    [HttpGet("test")]
    public void GetRole()
    {
        authContext.Roles.Where(r => r.Id == "4c6f4457-2bc8-41fa-a659-0f3251ca100b")
            .Select(r => r.Name)
            .SingleAsync()
            .ContinueWith(res => Console.WriteLine(res.Result));
    }
    
    [HttpGet("add-roles")]
    public IActionResult AddRoles()
    {
        userService.AddRoles();
        return Ok();
    }
    
    [HttpPost("{userId}/SetRole")]
    public ActionResult<UserResponse> MapUserRole([FromRoute] string userId, [FromQuery] string roleId)
    {
        return Ok(userService.MapUserRole(userId, roleId));
    }
    
    [HttpGet("{userId}")]
    public ActionResult<UserResponse> GetUser(string userId)
    {
        return Ok(userService.GetUser(userId));
    }
}
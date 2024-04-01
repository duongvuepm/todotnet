using Microsoft.AspNetCore.Identity;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class UserService(AuthContext authContext, [FromKeyedServices("UserRepository")] IRepository<MyUser, string> userRepository)
{
    public void AddRoles()
    {
        authContext.Roles.Add(new IdentityRole("Admin"));
        authContext.Roles.Add(new IdentityRole("Member"));
        authContext.SaveChanges();
    }

    public UserResponse MapUserRole(string userId, string roleId)
    {
        MyUser user = authContext.Users.Find(userId) ?? throw new ResourceNotFoundException($"User id {userId} not found.");

        var role = authContext.Roles.Where(r => r.Id == roleId)
            .Select(r => r)
            .Single() ?? throw new ResourceNotFoundException($"Role id {roleId} not found.");

        
        user.Roles.Add(role);
        authContext.SaveChanges();

        return GetUser(userId);
    }

    public UserResponse GetUser(string userId)
    {
        MyUser user = userRepository.GetById(userId);

        return new UserResponse()
        {
            Id = userId,
            Email = user.Email,
            Roles = user.Roles.Select(r => new RoleResponse(r.Id, r.Name)).ToList()
        };
    }
}
using Microsoft.AspNetCore.Identity;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class UserService(
    AuthContext authContext,
    UserManager<MyUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task<IEnumerable<RoleResponse>> AddRoles()
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
        await roleManager.CreateAsync(new IdentityRole("Member"));

        return roleManager.Roles.Select(r => new RoleResponse(r.Id, r.Name)).ToList();
    }

    public async Task<UserResponse> MapUserRole(string userId, string roleId)
    {
        MyUser user = authContext.Users.Find(userId) ??
                      throw new ResourceNotFoundException($"User id {userId} not found.");

        var role = authContext.Roles.Where(r => r.Id == roleId)
            .Select(r => r)
            .SingleOrDefault() ?? throw new ResourceNotFoundException($"Role id {roleId} not found.");

        await userManager.AddToRoleAsync(user, role.Name);

        return await GetUser(userId);
    }

    public async Task<UserResponse> SetUserRole(string username, string roleName)
    {
        MyUser user = await userManager.FindByNameAsync(username) ??
                      throw new ResourceNotFoundException($"User {username} not found.");
        IdentityRole role = await roleManager.FindByNameAsync(roleName) ??
                            throw new ResourceNotFoundException($"Role {roleName} not found.");

        await userManager.AddToRoleAsync(user, role.Name);

        return await userManager.FindByNameAsync(username)
            .ContinueWith(res =>
            {
                MyUser user = res.Result;
                return new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email
                    // Roles = user.Roles.Select(r => new RoleResponse(r.Id, r.Name)).ToList()
                };
            });
    }

    public async Task<UserResponse> GetUser(string userId)
    {
        MyUser user = await userManager.FindByIdAsync(userId) ??
                      throw new ResourceNotFoundException($"User id {userId} not found.");

        return new UserResponse()
        {
            Id = userId,
            Email = user.Email,
            Roles = user.Roles.Select(r => new RoleResponse(r.Id, r.Name)).ToList()
        };
    }
}
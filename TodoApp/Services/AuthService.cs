using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class AuthService(AuthContext authContext)
{
    public void AddRoles()
    {
        authContext.Roles.Add(new IdentityRole("Admin"));
        authContext.Roles.Add(new IdentityRole("Member"));
        authContext.SaveChanges();
    }

    public void MapUserRole(string userId, string roleId)
    {
        MyUser user = authContext.Users.Find(userId) ?? throw new ResourceNotFoundException($"User id {userId} not found.");

        IdentityRole role = authContext.Roles.Find(roleId) ?? throw new ResourceNotFoundException($"Role id {roleId} not found.");
            
        user.
    }
}
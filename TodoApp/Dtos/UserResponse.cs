using Microsoft.AspNetCore.Identity;

namespace TodoApp.Dtos;

public class UserResponse
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<RoleResponse> Roles { get; set; } = new List<RoleResponse>();

    public UserResponse()
    {
    }
}

public record RoleResponse(string Id, string Name)
{
    
}
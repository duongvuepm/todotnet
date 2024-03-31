using Microsoft.AspNetCore.Identity;

namespace TodoApp.Models;

public class MyUser : IdentityUser
{
    public ICollection<IdentityRole> Roles { get; set; }
}
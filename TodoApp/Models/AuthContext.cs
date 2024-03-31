using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class AuthContext(DbContextOptions<AuthContext> options) : IdentityDbContext<MyUser>(options)
{
}
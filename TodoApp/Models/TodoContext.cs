using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class TodoContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<State> States { get; set; } = null!;
    public string DbPath { get; }
    
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "todo.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<State>()
            .HasMany(s => s.TodoItems)
            .WithOne(t => t.State)
            .HasForeignKey(t => t.StateId);
    }
}
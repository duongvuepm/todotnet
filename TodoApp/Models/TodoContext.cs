using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class TodoContext : DbContext
{
    public DbSet<Item> TodoItems { get; set; } = null!;
    public DbSet<State> States { get; set; } = null!;
    public DbSet<Board> Boards { get; set; } = null!;
    public DbSet<Transition> Transitions { get; set; } = null!;

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

        modelBuilder.Entity<Board>()
            .HasMany(b => b.Items)
            .WithOne(t => t.Board)
            .HasForeignKey(t => t.BoardId);

        modelBuilder.Entity<Board>()
            .HasMany(b => b.States)
            .WithOne(s => s.Board)
            .HasForeignKey(s => s.BoardId);

        modelBuilder.Entity<Transition>()
            .HasOne(t => t.FromState)
            .WithMany()
            .HasForeignKey(t => t.FromStateId);

        modelBuilder.Entity<Transition>()
            .HasOne(t => t.ToState)
            .WithMany()
            .HasForeignKey(t => t.ToStateId);
    }
}
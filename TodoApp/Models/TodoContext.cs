﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models.Converters;

namespace TodoApp.Models;

public class TodoContext : DbContext
{
    public virtual DbSet<Item> TodoItems { get; set; } = null!;
    public virtual DbSet<State> States { get; set; } = null!;
    public DbSet<Board> Boards { get; set; } = null!;
    public virtual DbSet<Transition> Transitions { get; set; } = null!;

    public string DbPath { get; }

    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "todo.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .Property(i => i.DueDate).HasConversion<DateConverter>();

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

        modelBuilder.Entity<State>()
            .HasMany(s => s.Transitions)
            .WithOne(t => t.FromState)
            .HasForeignKey(t => t.FromStateId);
    }
}
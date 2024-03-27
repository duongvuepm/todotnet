using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace TodoApp.Models;

[Table("board")]
public class Board
{
    
    [Column("id")]
    public long Id { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    public ICollection<Item> Items { get; set; } = new List<Item>();
    
    public ICollection<State> States { get; set; } = new List<State>();
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

[Table("TodoItems")]
public class Item
{
    [Key] public long Id { get; set; }

    [Column("Name", TypeName = "varchar(100)")]
    [Required]
    public string? Name { get; set; }

    [Column("IsComplete")]
    [DefaultValue(false)]
    public bool IsComplete { get; set; }

    [Column("CreatedTimestamp")]
    [DefaultValue("datetime('now')")]
    public DateTime CreatedTimestamp { get; set; }

    public State State { get; set; } = null!;

    [ForeignKey("state_id")] public long StateId { get; set; }

    public Board Board { get; set; }

    [Column("board_id")]
    [ForeignKey("board_id")]
    public long? BoardId { get; set; }
    
    [Column("due_date")]
    public DateOnly? DueDate { get; set; }
}
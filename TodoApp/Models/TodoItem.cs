using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

[Table("TodoItems")]
public class TodoItem
{
    [Key]
    public long Id { get; set; }

    [Column("Name", TypeName = "varchar(100)")]
    [Required] public string? Name { get; set; }

    [Column("IsComplete")]
    [DefaultValue(false)] public bool IsComplete { get; set; }

    [Column("CreatedTimestamp")]
    [DefaultValue("datetime('now')")]
    public DateTime CreatedTimestamp { get; set; }
    
    public State State { get; set; } = null!;

    [ForeignKey("state_id")] public long StateId { get; set; }
}
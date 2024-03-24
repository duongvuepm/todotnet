using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

[Table("state")]
public class State
{
    [Key] [Column("id")] public long Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; } = null!;

    public virtual ICollection<State> Transitions { get; set; } = new List<State>();

    [Column("previous_state_id")] public long? PreviousStateId { get; set; }

    [DefaultValue(false)]
    [Column("is_default")]
    public bool IsDefault { get; set; }

    public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
}
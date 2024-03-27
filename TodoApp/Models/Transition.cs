using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

[Table("transition")]
public class Transition
{
    [Column("id")]
    public long Id { get; set; }

    [Column("from_state")]
    [ForeignKey("from_state_id")]
    public long FromStateId { get; set; }

    [Column("to_state")]
    [ForeignKey("to_state_id")]
    public long ToStateId { get; set; }
    
    public State FromState { get; set; }
    
    public State ToState { get; set; }
}

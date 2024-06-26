﻿using System.Collections;
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

    [DefaultValue(false)]
    [Column("is_default")]
    public bool IsDefault { get; set; }

    public virtual ICollection<Item> TodoItems { get; set; } = new List<Item>();

    public virtual Board Board { get; set; }

    [Column("board_id")]
    [ForeignKey("board_id")]
    public long? BoardId { get; set; }

    public virtual ICollection<Transition> Transitions { get; set; } = new List<Transition>();
}
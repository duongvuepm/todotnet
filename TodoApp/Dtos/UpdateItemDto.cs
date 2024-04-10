using System.Text.Json.Serialization;
using TodoApp.Dtos.Converters;
using TodoApp.Dtos.Validators;

namespace TodoApp.Dtos;

public class UpdateItemDto
{
    public string? Name { get; set; }

    [FutureDate]
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly? DueDate { get; set; }
}
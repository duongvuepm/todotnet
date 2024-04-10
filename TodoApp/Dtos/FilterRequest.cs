namespace TodoApp.Dtos;

public class FilterRequest
{
    public string? State { get; set; }
    public bool? Expired { get; set; }
}
namespace TodoApp.Dtos;

public record StateDto(string? Name, bool IsDefault, long? ParentStateId)
{
    
};
namespace TodoApp.Dtos;

public record StateDto(string? Name, bool IsDefault, long BoardId, long? ParentStateId)
{
    
};
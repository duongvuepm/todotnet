namespace TodoApp.Dtos;

public record StateDto(string? Name, bool IsDefault, long BoardId, StateTransitionDto? Transition)
{
};

public record StateTransitionDto(long ParentStateId, string? RoleRequired)
{
}
namespace TodoApp.Dtos;

public record BoardResponse(long Id, string Name, string Description, ICollection<ItemResponse> Items)
{
};
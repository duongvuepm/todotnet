namespace TodoApp.Dtos;

public record BoardResponse(long Id, string Name, string Description, ICollection<ItemResponse> Items)
{
};

public class BoardItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long StateId { get; set; }
}
namespace TodoApp.Dtos;

public record ItemResponse(long Id, string Name, long StateId, string? State)
{
    public ItemResponse(long id, string name, long stateId) : this(id, name, stateId, null)
    {
    }
}
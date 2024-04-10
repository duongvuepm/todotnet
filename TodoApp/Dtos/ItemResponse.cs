using System.Text.Json.Serialization;
using TodoApp.Dtos.Converters;

namespace TodoApp.Dtos;

public class ItemResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long StateId { get; set; }
    public string? State { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly? DueDate { get; set; }

    public ItemResponse(long id, string name, long stateId, string? state)
    {
        Id = id;
        Name = name;
        StateId = stateId;
        State = state;
    }

    public ItemResponse(long id, string name, long stateId, string? state, DateOnly? dueDate) : this(id, name, stateId,
        state)
        => DueDate = dueDate;

    public ItemResponse(long id, string name, long stateId) : this(id, name, stateId, null)
    {
    }
}
namespace TodoApp.Dtos;

public record StateResponse(long Id, string Name, IEnumerable<long> transitions)
{
    
}
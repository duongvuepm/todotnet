using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services;

public class FilterService(TodoContext todoContext)
{
    public async Task<ICollection<ItemResponse>> FilterItems(FilterRequest filter, long boardId)
    {
        IQueryable<Item> query = todoContext.TodoItems
            .Where(i => i.BoardId == boardId)
            .AsQueryable();

        if (filter.State != null)
        {
            query = query.Where(i => i.State.Name == filter.State).AsQueryable();
        }

        if (filter.Expired != null)
        {
            query = (filter.Expired ?? false)
                ? query.Where(i => i.DueDate == null || i.DueDate < DateOnly.FromDateTime(DateTime.Now)).AsQueryable()
                : query.Where(i => i.DueDate != null && i.DueDate >= DateOnly.FromDateTime(DateTime.Now)).AsQueryable();
        }

        return await query.Select(i => new ItemResponse(i.Id, i.Name ?? "", i.StateId, i.State.Name, i.DueDate))
            .ToListAsync();
    }
}
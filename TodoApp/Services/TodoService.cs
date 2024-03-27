using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class TodoService(TodoContext context)
{
    public async Task<ActionResult<IEnumerable<ItemResponse>>> GetTodoItems(long boardId)
    {
        return await
            (from t in context.TodoItems
                where t.BoardId == boardId
                select new ItemResponse(t.Id, t.Name ?? "", t.StateId))
            .ToListAsync();
    }

    public async Task<ActionResult<ItemResponse>> GetTodoItem(long id)
    {
        var query = from item in context.TodoItems
            where item.Id == id
            select new ItemResponse(item.Id, item.Name ?? "", item.StateId);


        return await query.SingleOrDefaultAsync() ?? throw new ResourceNotFoundException($"Item with id {id} not found");
    }

    public async Task<IActionResult> PutTodoItem(long id, Item item)
    {
        context.Entry(item).State = EntityState.Modified;

        await context.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<ActionResult<ItemResponse>> PostTodoItem(ItemDto newItemDto)
    {
        long stateId = await context.States.Where(s => s.IsDefault).Select(s => s.Id).SingleAsync();

        Item newItem = new()
        {
            Name = newItemDto.Name,
            StateId = stateId,
            BoardId = newItemDto.BoardId,
            CreatedTimestamp = DateTime.Now
        };

        var persistedItem = context.TodoItems.Add(newItem).Entity;
        return await context.SaveChangesAsync().ContinueWith(_ => GetTodoItem(persistedItem.Id)).Result;
    }

    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return new NotFoundResult();
        }

        context.TodoItems.Remove(todoItem);
        await context.SaveChangesAsync();

        return new NoContentResult();
    }
}
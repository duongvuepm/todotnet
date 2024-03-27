using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services;

public class TodoService(TodoContext context)
{
    public async Task<ActionResult<IEnumerable<TodoResponse>>> GetTodoItems()
    {
        return await
            (from t in context.TodoItems
                select new TodoResponse(t.Id, t.Name ?? "", t.StateId))
            .ToListAsync();
    }

    public async Task<ActionResult<TodoResponse>> GetTodoItem(long id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            throw new KeyNotFoundException();
        }

        return new TodoResponse(todoItem.Id, todoItem.Name ?? "", todoItem.StateId);
    }

    public async Task<IActionResult> PutTodoItem(long id, Item item)
    {
        context.Entry(item).State = EntityState.Modified;

        await context.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<ActionResult<TodoResponse>> PostTodoItem(TodoDto newTodo)
    {
        long stateId = await context.States.Where(s => s.IsDefault).Select(s => s.Id).SingleAsync();

        Item newItem = new()
        {
            Name = newTodo.Name,
            StateId = stateId,
            CreatedTimestamp = DateTime.Now
        };

        var newTodoId = context.TodoItems.Add(newItem).Entity.Id;
        await context.SaveChangesAsync();

        return new CreatedAtActionResult(nameof(GetTodoItem), "Todo", new { id = newTodoId }, newTodo);
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
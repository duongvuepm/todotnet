using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services;

public class WorkflowService(TodoContext context)
{
    public async Task<ActionResult<TodoResponse>> TransitState(long todoId, long nextStateId)
    {

                     var todoItem = await (from todo in context.TodoItems
            where todo.Id == todoId
            select todo).SingleAsync() ?? throw new KeyNotFoundException();

        var nextState = await (from state in context.States
                           where state.Id == nextStateId
                               select state).SingleAsync() ?? throw new KeyNotFoundException();


        todoItem.StateId = nextState.Id;
        await context.SaveChangesAsync();

        return new TodoResponse(todoId, todoItem.Name ?? "", nextStateId);
    }
}

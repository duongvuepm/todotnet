using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class WorkflowService(TodoContext context)
{
    public TodoResponse TransitState(long todoId, long nextStateId)
    {
        var todoItem = (from todo in context.TodoItems
            where todo.Id == todoId
            select todo).Single() ?? throw new ResourceNotFoundException($"Todo item with ID {todoId} not found");
        
        var currentNext = from s in context.States
            join s2 in context.States on s.Id equals s2.PreviousStateId
            select new { current = s, next = s2 };

        var todoNext = from t in context.TodoItems
            join s in currentNext on t.StateId equals s.current.Id
            where t.Id == todoId && s.next.Id == nextStateId
            select s.next.Id;
        
        if (!todoNext.Any())
            throw new InvalidStateTransitionException(
                $"Cannot transition item {todoId} to state {nextStateId}");

        todoItem.StateId = todoNext.Single();
        context.SaveChanges();

        return new TodoResponse(todoId, todoItem.Name ?? "", nextStateId);
    }
}
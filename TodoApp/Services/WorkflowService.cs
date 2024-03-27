using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class WorkflowService(TodoContext context)
{
    public ItemResponse TransitStateOtherWay(long todoId, long toStateId)
    {
        var todoItem = (from todo in context.TodoItems
            where todo.Id == todoId
            select todo).Single() ?? throw new ResourceNotFoundException($"Todo item with ID {todoId} not found");

        var transitionQuery = from cr in context.States
            join tr in context.Transitions on cr.Id equals tr.FromStateId
            join next in context.States on tr.ToStateId equals next.Id
            where next.Id == toStateId && cr.Id == todoItem.StateId
            select new {cr, next};

        if (transitionQuery.Any())
        {
            todoItem.StateId = toStateId;
            context.SaveChanges();

            return new ItemResponse(todoId, todoItem.Name ?? "", toStateId);
        }
        else
        {
            throw new InvalidStateTransitionException(
                               $"Cannot transition item {todoId} to state {toStateId}");
        }
    }
}
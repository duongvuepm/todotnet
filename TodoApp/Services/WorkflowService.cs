﻿using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class WorkflowService(TodoContext context)
{
    public ItemResponse TransitState(long todoId, long toStateId, string role)
    {
        var todoItem = (from todo in context.TodoItems
                           where todo.Id == todoId
                           select todo).SingleOrDefault() ??
                       throw new ResourceNotFoundException($"Todo item with ID {todoId} not found");

        var transitionQuery = from cr in context.States
            join tr in context.Transitions on cr.Id equals tr.FromStateId
            join next in context.States on tr.ToStateId equals next.Id
            where next.Id == toStateId && cr.Id == todoItem.StateId
            select new { cr, next, tr.RoleRequired };

        var result = transitionQuery.SingleOrDefault()
                     ?? throw new InvalidStateTransitionException(
                         $"Cannot transition item {todoId} to state {toStateId}");

        var roleRequired = result.RoleRequired;
        if (roleRequired != null && roleRequired.Split(",").All(r => r != role))
        {
            throw new ActionNotAllowedException(
                $"Role {role} is not allowed to transition item {todoId} to state {toStateId}");
        }

        todoItem.StateId = toStateId;
        context.SaveChanges();

        return new ItemResponse(todoId, todoItem.Name ?? "", toStateId);
    }
}
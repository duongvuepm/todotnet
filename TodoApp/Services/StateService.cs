using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class StateService(TodoContext context)
{
    public IEnumerable<StateResponse> GetAllStates(long boardId)
    {
        var stateQuery = from s in context.States
            join tr in context.Transitions on s.Id equals tr.FromStateId into transitions
            where s.BoardId == boardId
            select new StateResponse(s.Id, s.Name ?? "", transitions.Select(t => t.ToStateId).ToList());

        return stateQuery.ToList();
    }

    public StateResponse GetState(long stateId)
    {
        var query = from s in context.States
            join tr in context.Transitions on s.Id equals tr.FromStateId into transitions
                    where s.Id == stateId
                select new StateResponse(s.Id, s.Name ?? "", transitions.Select(t => t.ToStateId).ToList());

        return query.Single() ?? throw new ResourceNotFoundException($"State with ID {stateId} not found");
    }

    public StateResponse CreateState(StateDto stateRequest)
    {
        var state = new State
        {
            Name = stateRequest.Name ?? "",
            IsDefault = stateRequest.IsDefault,
            BoardId = stateRequest.BoardId,
            PreviousStateId = stateRequest.ParentStateId
        };

        State persistedState = context.States.Add(state).Entity;

        if (stateRequest.ParentStateId is long fromStateId)
        {
            context.Transitions.Add(new Transition
            {
                FromStateId = fromStateId,
                ToState = persistedState
            });
        }
        
        context.SaveChanges();

        return GetState(persistedState.Id);
    }

    public StateResponse UpdateState(StateDto stateRequest, long stateId)
    {
        var currentState =
            (from s in context.States
                where s.Id == stateId
                select s)
            .Single() ?? throw new ResourceNotFoundException($"State with ID {stateId} not found");

        currentState.Name = stateRequest.Name ?? currentState.Name;

        context.States.Update(currentState);
        context.SaveChanges();

        return GetState(stateId);
    }
}
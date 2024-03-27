using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class TransitionService(TodoContext dbContext, StateService stateService)
{
    public StateResponse AddTransition(long fromState, long toState)
    {
        var checkTransition = from tr in dbContext.Transitions
                    where tr.FromStateId == fromState && tr.ToStateId == toState
                        select tr;

        var checkStateRequest = from s in dbContext.States
                     where s.Id == fromState || s.Id == toState
                     select s;

        if (checkTransition.Any())
        {
            throw new ResourceAlreadyExistException($"Transition from {fromState} to {toState} already exists");
        } 

        if (checkStateRequest.Count() != 2)
        {
            throw new ResourceNotFoundException("At least one state not found");
        }

        Transition transition = new()
        {
            FromStateId = fromState,
            ToStateId = toState
        };

        dbContext.Transitions.Add(transition);
        dbContext.SaveChanges();

        return stateService.GetState(fromState);
    }
}

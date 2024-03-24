using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class StateService(TodoContext context)
{
    public IEnumerable<StateResponse> GetAllStates()
    {
        var query3 = from s in
                (from state in context.States
                    select new
                    {
                        state,
                        transitions = from t in state.Transitions select t.Id
                    })
            select new StateResponse(s.state.Id, s.state.Name ?? "", s.transitions.ToList());

        return query3.ToList();
    }

    public StateResponse GetState(long stateId)
    {
        var stateQuery = from s in context.States
            where s.Id == stateId
            select new StateResponse(s.Id, s.Name ?? "", s.Transitions.Select(t => t.Id).ToList());

        return stateQuery.Single() ?? throw new ResourceNotFoundException($"State with ID {stateId} not found");
    }

    public StateResponse CreateState(StateDto stateRequest)
    {
        var state = new State
        {
            Name = stateRequest.Name ?? "",
            IsDefault = stateRequest.IsDefault,
            PreviousStateId = stateRequest.ParentStateId
        };

        context.States.Add(state);
        context.SaveChanges();

        return new StateResponse(state.Id, state.Name ?? "", state.Transitions.Select(t => t.Id).ToList());
    }

    public StateResponse UpdateState(StateDto stateRequest, long stateId)
    {
        var currentState =
            (from s in context.States
                where s.Id == stateId
                select s)
            .Single() ?? throw new ResourceNotFoundException($"State with ID {stateId} not found");

        currentState.Name = stateRequest.Name ?? currentState.Name;
        currentState.PreviousStateId = stateRequest.ParentStateId ?? currentState.PreviousStateId;

        context.States.Update(currentState);
        context.SaveChanges();

        return new StateResponse(currentState.Id, currentState.Name ?? "", currentState.Transitions.Select(t => t.Id).ToList());
    }
}
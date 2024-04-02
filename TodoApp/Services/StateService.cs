using TodoApp.Dtos;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class StateService(IStateRepository stateRepository, [FromKeyedServices("TransitionRepository")] IRepository<Transition, long> transitionRepository)
{
    public IEnumerable<StateResponse> GetAllStates(long boardId)
    {
        return stateRepository.GetAllStates(boardId)
            .Select(s => new StateResponse(s.Id, s.Name ?? "", s.Transitions.Select(t => t.ToStateId)))
            .ToList();
    }

    public StateResponse GetState(long stateId)
    {
        return stateRepository.GetByIdAsync(stateId)
            .ContinueWith(res =>
            {
                State state = res.Result;
                return new StateResponse(state.Id, state.Name ?? "",
                    state.Transitions.Select(t => t.ToStateId).ToList());
            })
            .Result;
    }

    public StateResponse CreateState(StateDto stateRequest)
    {
        var state = new State
        {
            Name = stateRequest.Name ?? "",
            IsDefault = stateRequest.IsDefault,
            BoardId = stateRequest.BoardId
        };

        State persistedState = stateRepository.Create(state);

        if (stateRequest.ParentStateId is long fromStateId)
        {
            transitionRepository.Create(new Transition
            {
                FromStateId = fromStateId,
                ToStateId = persistedState.Id
            });
        }

        return GetState(persistedState.Id);
    }

    public StateResponse UpdateState(StateDto stateRequest, long stateId)
    {
        var currentState = stateRepository.GetById(stateId);

        currentState.Name = stateRequest.Name ?? currentState.Name;
        stateRepository.Update(currentState);

        return GetState(stateId);
    }
}
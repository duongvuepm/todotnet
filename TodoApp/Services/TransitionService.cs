using Microsoft.AspNetCore.Identity;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class TransitionService(
    IStateRepository stateRepository,
    [FromKeyedServices("TransitionRepository")]
    IRepository<Transition, long> transitionRepository,
    RoleManager<IdentityRole> roleManager)
{
    public async Task<StateResponse> AddTransition(long fromState, long toState, string roleRequired)
    {
        CheckRoleRequired(roleRequired);

        var transitionQuery = transitionRepository.Query(q => from tr in q
            where tr.FromStateId == fromState && tr.ToStateId == toState
            select tr);

        var checkStateRequest = stateRepository.Query(q => from s in q
            where s.Id == fromState || s.Id == toState
            select s);

        if (transitionQuery.Any())
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
            ToStateId = toState,
            RoleRequired = roleRequired
        };

        Console.WriteLine("saving transition");
        transitionRepository.Create(transition);

        return await stateRepository.GetByIdAsync(fromState)
            .ContinueWith(res =>
            {
                State state = res.Result;
                return new StateResponse(state.Id, state.Name ?? "",
                    state.Transitions.Select(t => t.ToStateId).ToList());
            });
    }

    public async Task<StateResponse> UpdateTransition(long transitionId, long? toState, string? roleRequired)
    {
        CheckRoleRequired(roleRequired);

        return await transitionRepository.GetByIdAsync(transitionId)
            .ContinueWith(res =>
            {
                Transition transition = res.Result;
                transition.RoleRequired = roleRequired ?? transition.RoleRequired;
                transition.ToStateId = toState ?? transition.ToStateId;

                return transitionRepository.Update(transition);
            })
            .ContinueWith(res =>
            {
                Transition transition = res.Result;
                return stateRepository.GetById(transition.FromStateId);
            })
            .ContinueWith(res =>
            {
                var state = res.Result;
                return new StateResponse(state.Id, state.Name ?? "",
                    state.Transitions.Select(t => t.ToStateId).ToList());
            });
    }

    private async void CheckRoleRequired(string roleRequired)
    {
        Console.WriteLine("Checking role");
        var role = await roleManager.FindByNameAsync(roleRequired);
        if (role == null)
        {
            throw new ResourceNotFoundException($"Role {roleRequired} not found");
        }
    }
}
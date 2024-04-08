using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StateController(StateService stateService, TransitionService transitionService) : ControllerBase
{
    [HttpGet, Authorize(Roles = "Member")]
    public ActionResult<IEnumerable<StateResponse>> GetAllStates([FromQuery] [BindRequired] long boardId)
    {
        return Ok(stateService.GetAllStates(boardId));
    }

    [HttpGet("{stateId}")]
    public ActionResult<StateResponse> GetState(long stateId)
    {
        return Ok(stateService.GetState(stateId));
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public ActionResult<StateResponse> CreateState(StateDto stateDto)
    {
        StateResponse createdState = stateService.CreateState(stateDto);

        return CreatedAtAction(nameof(GetState), new { stateId = createdState.Id }, createdState);
    }

    [HttpPut("{stateId}")]
    public ActionResult<StateResponse> UpdateState([FromRoute] long stateId, StateDto stateDto)
    {
        StateResponse updatedState = stateService.UpdateState(stateDto, stateId);

        return CreatedAtAction(nameof(GetState), new { stateId = updatedState.Id }, updatedState);
    }

    [HttpPut("{stateId}/AddTransition")]
    public async Task<ActionResult<StateResponse>> AddTransition([FromBody] TransitionDto addTransition,
        [FromRoute] long stateId)
    {
        if (addTransition.ToState == stateId)
            throw new InvalidStateTransitionException("Cannot transition to the same state");

        StateResponse updatedState =
            await transitionService.AddTransition(stateId, addTransition.ToState, addTransition.RoleRequired);
        return CreatedAtAction(nameof(GetState), new { stateId = updatedState.Id }, updatedState);
    }

    [HttpPut("{stateId}/Transitions/{transitionId}")]
    public async Task<ActionResult<StateResponse>> UpdateTransition([FromBody] TransitionDto addTransition,
        [FromRoute] long stateId, [FromRoute] long transitionId)
    {
        StateResponse updatedState =
            await transitionService.UpdateTransition(transitionId, addTransition.ToState, addTransition.RoleRequired);
        return CreatedAtAction(nameof(GetState), new { stateId }, updatedState);
    }
}
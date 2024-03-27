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
    [HttpGet]
    public ActionResult<IEnumerable<StateResponse>> GetAllStates([FromQuery][BindRequired] long boardId)
    {
        return Ok(stateService.GetAllStates(boardId));
    }

    [HttpGet("{stateId}")]
    public ActionResult<StateResponse> GetState(long stateId)
    {
        return Ok(stateService.GetState(stateId));
    }

    [HttpPost]
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
    public ActionResult<StateResponse> AddTransition([FromBody] TransitionDto addTransition, [FromRoute] long stateId)
    {
        if (addTransition.ToState == stateId) throw new InvalidStateTransitionException("Cannot transition to the same state");

        StateResponse updatedState = transitionService.AddTransition(stateId, addTransition.ToState);
        return CreatedAtAction(nameof(GetState), new { stateId = updatedState.Id }, updatedState);
    }
}
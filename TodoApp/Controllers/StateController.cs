using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StateController(StateService stateService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<StateResponse>> GetAllStates()
    {
        return Ok(stateService.GetAllStates());
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
}
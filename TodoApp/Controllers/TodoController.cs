using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.Middlewares;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TodoController(TodoService todoService, WorkflowService workflowService) : ControllerBase
{
    // GET: api/Todo
    /// <summary>
    /// Get all TodoItems
    /// </summary>
    /// <returns> A list of TodoItems</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoResponse>>> GetTodoItems()
    {
        return await todoService.GetTodoItems();
    }

    // GET: api/Todo/5
    /// <summary>
    /// Get a TodoItem by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An TodoItem with specified ID</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoResponse>> GetTodoItem(long id)
    {
        return await todoService.GetTodoItem(id);
    }

    // PUT: api/Todo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
    {
        return await todoService.PutTodoItem(id, todoItem);
    }

    /// <summary>
    /// Create new TodoItem
    /// </summary>
    /// <param name="todoItem"></param>
    /// <returns>Newly created TodoItem</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
    ///     {
    ///        "id": 1,
    ///        "name": "Item #1",
    ///        "isComplete": true
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Return a newly create TodoItem</response>
    /// <response code="400">Bad Request when the item is null</response>
    // POST: api/Todo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoResponse>> PostTodoItem(TodoDto todoItem)
    {
        return await todoService.PostTodoItem(todoItem);
    }

    // DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        return await todoService.DeleteTodoItem(id);
    }
    
    [HttpPut("{id}/ChangeState")]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
    public ActionResult<TodoResponse>TransitState([FromRoute] long id, [FromQuery] long nextStateId)
    {
        TodoResponse newTodoState = workflowService.TransitState(id, nextStateId);
        
        return CreatedAtAction(nameof(GetTodoItem), new { id = newTodoState.Id }, newTodoState);
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("/api/boards")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BoardController(BoardService boardService) : ControllerBase
{

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<BoardResponse> GetBoard(long id)
    {
        return Ok(boardService.GetBoard(id));
    }
    
    [HttpPost]
    public ActionResult<BoardResponse> CreateBoard([FromBody] BoardDto boardDto)
    {
        var createdBoar = boardService.CreateBoard(boardDto);
        return CreatedAtAction(nameof(GetBoard), new { id = createdBoar.Id }, createdBoar);
    }
}
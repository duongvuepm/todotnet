﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.Services;

namespace TodoApp.Controllers;

[ApiController]
[Route("/api/boards")]
public class BoardController(BoardService boardService, FilterService filterService) : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<BoardResponse> GetBoard(long id)
    {
        string role = User.FindFirst(ClaimTypes.Role)?.Value;
        Console.WriteLine($"User {role}");

        return Ok(boardService.GetBoard(id));
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public ActionResult<BoardResponse> CreateBoard([FromBody] BoardDto boardDto)
    {
        var createdBoar = boardService.CreateBoard(boardDto);
        return CreatedAtAction(nameof(GetBoard), new { id = createdBoar.Id }, createdBoar);
    }

    [HttpPost("{id}/QueryItems"), Authorize(Roles = "Admin, Member")]
    public async Task<ActionResult<ICollection<ItemResponse>>> FilterItems([FromBody] FilterRequest filterQuery,
        [FromRoute] long id)
    {
        var filteredItems = await filterService.FilterItems(filterQuery, id);
        return Ok(filteredItems);
    }
}
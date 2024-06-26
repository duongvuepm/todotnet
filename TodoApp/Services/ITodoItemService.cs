﻿using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services;

public interface ITodoItemService
{
    public Task<IEnumerable<ItemResponse>> GetTodoItems(long boardId);

    public Task<ItemResponse> GetTodoItem(long id);

    public Task<ItemResponse> UpdateItem(long id, UpdateItemDto item);

    public Task<ItemResponse> PostTodoItem(ItemDto newItemDto);

    public Task<IActionResult> DeleteTodoItem(long id);
}
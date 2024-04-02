using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Dtos;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class ItemService(
    [FromKeyedServices("ItemRepository")] IRepository<Item, long> itemRepository,
    [FromKeyedServices("StateRepository")] IRepository<State, long> stateRepository) : ITodoItemService
{
    public async Task<IEnumerable<ItemResponse>> GetTodoItems(long boardId)
    {
        return await itemRepository.Query(items => items.Where(i => i.BoardId == boardId))
            .ToListAsync()
            .ContinueWith(res =>
                res.Result.Select(item => new ItemResponse(item.Id, item.Name ?? "", item.StateId)).ToList());
    }

    public Task<ItemResponse> GetTodoItem(long id)
    {
        return itemRepository.GetByIdAsync(id)
            .ContinueWith(res => new ItemResponse(res.Result.Id, res.Result.Name ?? "", res.Result.StateId));
    }

    public Task<IActionResult> PutTodoItem(long id, Item item)
    {
        return itemRepository.GetByIdAsync(id)
            .ContinueWith(res =>
            {
                Item currentItem = res.Result;

                currentItem.Name = item.Name;
                currentItem.IsComplete = item.IsComplete;

                return currentItem;
            })
            .ContinueWith(res => itemRepository.Update(res.Result))
            .ContinueWith<IActionResult>(_ => new NoContentResult());
    }

    public Task<ItemResponse> PostTodoItem(ItemDto newItemDto)
    {
        return stateRepository.Query(states => states.Where(s => s.IsDefault))
            .SingleAsync()
            .ContinueWith(defaultState => new Item()
            {
                BoardId = newItemDto.BoardId,
                Name = newItemDto.Name,
                State = defaultState.Result
            })
            .ContinueWith(res => itemRepository.Create(res.Result))
            .ContinueWith(res => new ItemResponse(res.Result.Id, res.Result.Name ?? "", res.Result.StateId));
    }

    public Task<IActionResult> DeleteTodoItem(long id)
    {
        return Task.Factory.StartNew(() => itemRepository.Delete(id))
            .ContinueWith<IActionResult>(_ => new NoContentResult());
    }
}
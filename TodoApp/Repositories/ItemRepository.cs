using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class ItemRepository(TodoContext dbContext) : IRepository<Item, long>, IQueryRepository<Item>
{
    public IEnumerable<Item> GetAll()
    {
        return dbContext.TodoItems.ToList();
    }

    public Item GetById(long id)
    {
        return dbContext.TodoItems.Find(id) ?? throw new ResourceNotFoundException($"Item with id {id} not found");
    }

    public Item Create(Item entity)
    {
        return dbContext.TodoItems.Add(entity).Entity;
    }

    public Item Update(Item entity)
    {
        return dbContext.TodoItems.Update(entity).Entity;
    }

    public void Delete(long id)
    {
        dbContext.TodoItems.Where(i => i.Id == id).ExecuteDelete();
    }

    public IQueryable<Item> Query(Func<IQueryable<Item>, IQueryable<Item>> query)
    {
        return query(dbContext.TodoItems);
    }
}
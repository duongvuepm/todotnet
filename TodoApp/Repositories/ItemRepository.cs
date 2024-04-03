using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class ItemRepository(TodoContext dbContext) : IRepository<Item, long>
{
    public IEnumerable<Item> GetAll()
    {
        return dbContext.TodoItems.ToList();
    }

    public Item GetById(long id)
    {
        return dbContext.TodoItems
            .Where(i => i.Id == id)
            .Include(i => i.State)
            .SingleOrDefault() ?? throw new ResourceNotFoundException($"Item with id {id} not found");
    }

    public Item Create(Item entity)
    {
        Item newItem = dbContext.TodoItems.Add(entity).Entity;
        dbContext.SaveChanges();
        return newItem;
    }

    public Item Update(Item entity)
    {
        Item newItem = dbContext.TodoItems.Update(entity).Entity;
        dbContext.SaveChanges();
        return newItem;
    }

    public void Delete(long id)
    {
        dbContext.TodoItems.Where(i => i.Id == id).ExecuteDelete();
        dbContext.SaveChanges();
    }

    public IQueryable<Item> Query(Func<IQueryable<Item>, IQueryable<Item>> query)
    {
        return query(dbContext.TodoItems);
    }
}
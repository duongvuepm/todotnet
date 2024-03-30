using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class StateRepository(TodoContext dbContext) : IRepository<State, long>
{
    public IEnumerable<State> GetAll()
    {
        return dbContext.States.ToList();
    }

    public State GetById(long id)
    {
        return dbContext.States.Find(id) ?? throw new ResourceNotFoundException($"State with id {id} not found");
    }

    public State Create(State entity)
    {
        return dbContext.States.Add(entity).Entity;
    }

    public State Update(State entity)
    {
        return dbContext.States.Update(entity).Entity;
    }

    public void Delete(long id)
    {
        dbContext.States.Where(s => s.Id == id).ExecuteDelete();
    }
}
using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class TransitionRepository(TodoContext dbContext) : IRepository<Transition, long>
{
    public IEnumerable<Transition> GetAll()
    {
        return dbContext.Transitions.ToList();
    }

    public Transition GetById(long id)
    {
        return dbContext.Transitions.Find(id) ?? throw new ResourceNotFoundException($"Transition with id {id} not found");
    }

    public Transition Create(Transition entity)
    {
        return dbContext.Transitions.Add(entity).Entity;
    }

    public Transition Update(Transition entity)
    {
        return dbContext.Transitions.Update(entity).Entity;
    }

    public void Delete(long id)
    {
        dbContext.Transitions.Where(t => t.Id == id).ExecuteDelete();
    }
}
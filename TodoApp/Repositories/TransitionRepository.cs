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
        return dbContext.Transitions.Find(id) ??
               throw new ResourceNotFoundException($"Transition with id {id} not found");
    }

    public Transition Create(Transition entity)
    {
        Transition newTransition = dbContext.Transitions.Add(entity).Entity;
        dbContext.SaveChanges();
        return newTransition;
    }

    public Transition Update(Transition entity)
    {
        Transition newTransition = dbContext.Transitions.Update(entity).Entity;
        dbContext.SaveChanges();
        return newTransition;
    }

    public void Delete(long id)
    {
        dbContext.Transitions.Where(t => t.Id == id).ExecuteDelete();
        dbContext.SaveChanges();
    }

    public IQueryable<Transition> Query(Func<IQueryable<Transition>, IQueryable<Transition>> query)
    {
        return query(dbContext.Transitions);
    }
}
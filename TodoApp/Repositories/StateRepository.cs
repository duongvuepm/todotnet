using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Repositories;

public class StateRepository(TodoContext dbContext) : IStateRepository
{
    public IEnumerable<State> GetAll()
    {
        return dbContext.States.ToList();
    }

    public State GetById(long id)
    {
        return dbContext.States
            .Where(s => s.Id == id)
            .Include(s => s.Transitions)
            .SingleOrDefault() ?? throw new ResourceNotFoundException($"State with id {id} not found");
    }

    public State Create(State entity)
    {
        State newState = dbContext.States.Add(entity).Entity;
        dbContext.SaveChanges();
        return newState;
    }

    public State Update(State entity)
    {
        State newState = dbContext.States.Update(entity).Entity;
        dbContext.SaveChanges();
        return newState;
    }

    public void Delete(long id)
    {
        dbContext.States.Where(s => s.Id == id).ExecuteDelete();
        dbContext.SaveChanges();
    }

    public IEnumerable<State> GetAllStates(long boardId)
    {
        var stateQuery = dbContext.States
            .Include(s => s.Transitions)
            .Where(s => s.BoardId == boardId);

        return stateQuery.ToList();
    }

    public IQueryable<State> Query(Func<IQueryable<State>, IQueryable<State>> query)
    {
        return query(dbContext.States);
    }
}
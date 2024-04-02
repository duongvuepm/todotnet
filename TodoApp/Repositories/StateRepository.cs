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
                .Single() ?? throw new ResourceNotFoundException($"State with id {id} not found");
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

    public IEnumerable<State> GetAllStates(long boardId)
    {
        var stateQuery = from s in dbContext.States
            join tr in dbContext.Transitions on s.Id equals tr.FromStateId into transitions
            where s.BoardId == boardId
            select s;

        var anotherQuery = dbContext.States
            .Include(s => s.Transitions)
            .Where(s => s.BoardId == boardId);

        return anotherQuery.ToList();
    }
}
using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class BoardRepository(TodoContext dbContext) : IRepository<Board, long>
{
    public IEnumerable<Board> GetAll()
    {
        return dbContext.Boards.ToList();
    }

    public Board GetById(long id)
    {
        return dbContext.Boards.Find(id) ?? throw new ResourceNotFoundException($"Board with id {id} not found");
    }

    public Board Create(Board entity)
    {
        return dbContext.Boards.Add(entity).Entity;
    }

    public Board Update(Board entity)
    {
        return dbContext.Boards.Update(entity).Entity;
    }

    public void Delete(long id)
    {
        dbContext.Boards.Where(b => b.Id == id).ExecuteDelete();
    }
}
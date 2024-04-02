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
        return dbContext.Boards
            .Where(b => b.Id == id)
            .Select(b => b)
            .Include(b => b.Items)
            .Single() ?? throw new ResourceNotFoundException($"Board with id {id} not found");
    }

    public Board Create(Board entity)
    {
        Board newBoard = dbContext.Boards.Add(entity).Entity;
        dbContext.SaveChanges();
        return newBoard;
    }

    public Board Update(Board entity)
    {
        Board updatedBoard = dbContext.Boards.Add(entity).Entity;
        dbContext.SaveChanges();
        return updatedBoard;
    }

    public void Delete(long id)
    {
        dbContext.Boards.Where(b => b.Id == id).ExecuteDelete();
        dbContext.SaveChanges();
    }

    public IQueryable<Board> Query(Func<IQueryable<Board>, IQueryable<Board>> query)
    {
        return query(dbContext.Boards);
    }
}
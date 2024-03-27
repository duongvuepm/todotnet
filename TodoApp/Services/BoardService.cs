using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class BoardService(TodoContext dbContext)
{
    public BoardResponse GetBoard(long id)
    {
        var testQuery = from b in dbContext.Boards
            join i in dbContext.TodoItems on b.Id equals i.BoardId into boardItems
            where b.Id == id
            select new
            {
                b.Id,
                b.Name,
                b.Description,
                ItemIds = from bi in boardItems select new ItemResponse(bi.Id, bi.Name ?? "", bi.StateId)
            };
        
        var board = testQuery.Single() ?? throw new ResourceNotFoundException($"Board with ID {id} not found");

        return new BoardResponse(board.Id, board.Name, board.Description, board.ItemIds.ToList());

    }

    public BoardResponse CreateBoard(BoardDto boardDto)
    {
        Board newBoard = new Board()
        {
            Name = boardDto.Name,
            Description = boardDto.Description ?? ""
        };
        
        dbContext.Boards.Add(newBoard);
        dbContext.SaveChanges();
        
        return GetBoard(newBoard.Id);
    }
}
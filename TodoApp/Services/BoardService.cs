using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class BoardService([FromKeyedServices("BoardRepository")] IRepository<Board, long> boardRepository)
{
    public BoardResponse GetBoard(long id)
    {
        return boardRepository.GetByIdAsync(id)
            .ContinueWith(res =>
            {
                var board = res.Result;
                return new BoardResponse(board.Id, board.Name, board.Description ?? "",
                    board.Items.Select(i => new ItemResponse(i.Id, i.Name ?? "", i.StateId)).ToList()
                );
            })
            .Result;
    }

    public BoardResponse CreateBoard(BoardDto boardDto)
    {
        Board newBoard = new Board()
        {
            Name = boardDto.Name,
            Description = boardDto.Description ?? ""
        };

        Board createdBoard = boardRepository.Create(newBoard);
        
        return GetBoard(createdBoard.Id);
    }
}
using TodoApp.Dtos;
using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class BoardService([FromKeyedServices("BoardRepository")] IRepository<Board, long> boardRepository, [FromKeyedServices("ItemService")] ITodoItemService itemService)
{
    public BoardResponse GetBoard(long id)
    {
        return itemService.GetTodoItems(id)
            .ContinueWith(res => new
            {
                board = boardRepository.GetById(id),
                items = res.Result
            })
            .ContinueWith(res =>
            {
                var boardWithItems = res.Result;
                var board = boardWithItems.board;

                return new BoardResponse(board.Id, board.Name, board.Description ?? "",
                    (ICollection<ItemResponse>)boardWithItems.items);
            })
            .Result;
        //return boardRepository.GetByIdAsync(id)
        //    .ContinueWith(res =>
        //    {
        //        var board = res.Result;
        //        return new BoardResponse(board.Id, board.Name, board.Description ?? "",
        //            //board.Items.Select(i => new BoardItem
        //            //{
        //            //    Id = i.Id,
        //            //    Name = i.Name ?? "",
        //            //    StateId = i.StateId
        //            //}).ToList()
        //            itemRepository.GetAll().
        //        );
        //    })
        //    .Result;
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
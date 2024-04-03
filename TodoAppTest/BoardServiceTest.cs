using FluentAssertions;
using Moq;
using TodoApp.Dtos;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Services;

namespace TodoAppTest;

public class BoardServiceTest
{
    private readonly Mock<IRepository<Board, long>> _boardRepositoryMock = new();
    private readonly Mock<ITodoItemService> _itemServiceMock = new();

    [Fact]
    public void GivenBoardIdNotExist_WhenGetBoardById_ThenThrowNotFoundException()
    {
        // Given
        _boardRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>())).Throws<ResourceNotFoundException>();
        var boardService = new BoardService(_boardRepositoryMock.Object, _itemServiceMock.Object);

        // When
        Action act = () => boardService.GetBoard(1);

        // Then
        act.Should().Throw<ResourceNotFoundException>();
    }

    [Fact]
    public void GivenBoardWithNoItems_WhenGetBoardById_ThenReturnBoardResponse()
    {
        // Given
        _boardRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>())).Returns(new Board());
        _itemServiceMock.Setup(service => service.GetTodoItems(It.IsAny<long>()))
            .ReturnsAsync(new List<ItemResponse>());
        var boardService = new BoardService(_boardRepositoryMock.Object, _itemServiceMock.Object);

        // When
        var result = boardService.GetBoard(1);

        // Then
        result.Should().NotBeNull()
            .And.BeOfType<BoardResponse>()
            .Which.Items.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void GivenBoardWithSomeItems_WhenGetBoardById_ThenReturnBoardResponseWithItems()
    {
        // Given
        _boardRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>())).Returns(new Board());
        _itemServiceMock.Setup(service => service.GetTodoItems(It.IsAny<long>())).ReturnsAsync(new List<ItemResponse>
        {
            new(1, "Item 1", 1, "State 1"),
            new(2, "Item 2", 2, "State 2")
        });
        var boardService = new BoardService(_boardRepositoryMock.Object, _itemServiceMock.Object);

        // When
        var result = boardService.GetBoard(1);

        // Then
        result.Should().NotBeNull()
            .And.BeOfType<BoardResponse>()
            .Which.Items.Should().NotBeNull().And.HaveCount(2);
    }
}
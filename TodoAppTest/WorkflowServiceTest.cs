using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Services;

namespace TodoAppTest;

public class WorkflowServiceTest
{
    private readonly Mock<TestTodoContext> _mockDbContext = new();

    [Fact]
    public void GivenNoItemFound_WhenTransitState_ThenThrowException()
    {
        // Given
        var data = new List<Item>
        {
            new() { Id = 1, Name = "Item1", StateId = 1 },
        }.AsQueryable();
        _mockDbContext.Setup(c => c.TodoItems).Returns(GetMockItemSet(data));
        IRepository<Item, long> itemRepository = new ItemRepository(_mockDbContext.Object);
        var workflowService = new WorkflowService(_mockDbContext.Object, itemRepository);

        // When
        Action changeStateAct = () => workflowService.TransitState(2, 2, "Admin");

        // Then
        changeStateAct.Should().Throw<ResourceNotFoundException>();
    }

    [Fact]
    public void GivenMatchedItemWithStateHavingNoTransition_WhenTransitState_ThenThrowException()
    {
        // Given
        var items = new List<Item>
        {
            new() { Id = 1, Name = "Item1", StateId = 1 },
        }.AsQueryable();
        var states = new List<State>
        {
            new() { Id = 1, Name = "State1" },
            new() { Id = 2, Name = "State2" },
        }.AsQueryable();
        var transitions = new List<Transition>().AsQueryable();
        _mockDbContext.Setup(c => c.TodoItems).Returns(GetMockItemSet(items));
        _mockDbContext.Setup(c => c.States).Returns(GetMockItemSet(states));
        _mockDbContext.Setup(c => c.Transitions).Returns(GetMockItemSet(transitions));
        IRepository<Item, long> itemRepository = new ItemRepository(_mockDbContext.Object);
        var workflowService = new WorkflowService(_mockDbContext.Object, itemRepository);

        // When
        Action changeStateAct = () => workflowService.TransitState(1, 2, "Admin");

        // Then
        changeStateAct.Should().Throw<InvalidStateTransitionException>();
    }

    [Fact]
    public void GivenMatchedItemWithStateHavingTransitionButRoleNotAllowed_WhenTransitState_ThenThrowException()
    {
        // Given
        var items = new List<Item>
        {
            new() { Id = 1, Name = "Item1", StateId = 1 },
        }.AsQueryable();
        var states = new List<State>
        {
            new() { Id = 1, Name = "State1" },
            new() { Id = 2, Name = "State2" },
        }.AsQueryable();
        var transitions = new List<Transition>
        {
            new() { FromStateId = 1, ToStateId = 2, RoleRequired = "Admin" },
        }.AsQueryable();
        _mockDbContext.Setup(c => c.TodoItems).Returns(GetMockItemSet(items));
        _mockDbContext.Setup(c => c.States).Returns(GetMockItemSet(states));
        _mockDbContext.Setup(c => c.Transitions).Returns(GetMockItemSet(transitions));
        IRepository<Item, long> itemRepository = new ItemRepository(_mockDbContext.Object);
        var workflowService = new WorkflowService(_mockDbContext.Object, itemRepository);

        // When
        Action changeStateAct = () => workflowService.TransitState(1, 2, "Member");

        // Then
        changeStateAct.Should().Throw<ActionNotAllowedException>();
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Member")]
    public void GivenMatchedItemWithStateAndTransitionHavingNoRoleRequired_WhenTransitState_ThenChangeState(string role)
    {
        // Given
        var items = new List<Item>
        {
            new() { Id = 1, Name = "Item1", StateId = 1 },
        }.AsQueryable();
        var states = new List<State>
        {
            new() { Id = 1, Name = "State1" },
            new() { Id = 2, Name = "State2" },
        }.AsQueryable();
        var transitions = new List<Transition>
        {
            new() { FromStateId = 1, ToStateId = 2 },
        }.AsQueryable();
        _mockDbContext.Setup(c => c.TodoItems).Returns(GetMockItemSet(items));
        _mockDbContext.Setup(c => c.States).Returns(GetMockItemSet(states));
        _mockDbContext.Setup(c => c.Transitions).Returns(GetMockItemSet(transitions));
        IRepository<Item, long> itemRepository = new ItemRepository(_mockDbContext.Object);
        var workflowService = new WorkflowService(_mockDbContext.Object, itemRepository);

        // When
        var result = workflowService.TransitState(1, 2, role);

        // Then
        result.Id.Should().Be(1);
        result.Name.Should().Be("Item1");
        result.StateId.Should().Be(2);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Member")]
    public void GivenItemWithStateTransitionHavingRightRoleRequired_WhenTransitState_ThenChangeState(string role)
    {
        // Given
        var items = new List<Item>
        {
            new() { Id = 1, Name = "Item1", StateId = 1 },
        }.AsQueryable();
        var states = new List<State>
        {
            new() { Id = 1, Name = "State1" },
            new() { Id = 2, Name = "State2" },
        }.AsQueryable();
        var transitions = new List<Transition>
        {
            new() { FromStateId = 1, ToStateId = 2, RoleRequired = role },
        }.AsQueryable();

        _mockDbContext.Setup(c => c.TodoItems).Returns(GetMockItemSet(items));
        _mockDbContext.Setup(c => c.States).Returns(GetMockItemSet(states));
        _mockDbContext.Setup(c => c.Transitions).Returns(GetMockItemSet(transitions));
        IRepository<Item, long> itemRepository = new ItemRepository(_mockDbContext.Object);
        var workflowService = new WorkflowService(_mockDbContext.Object, itemRepository);

        // When
        var result = workflowService.TransitState(1, 2, role);

        // Then
        result.Id.Should().Be(1);
        result.Name.Should().Be("Item1");
        result.StateId.Should().Be(2);
    }

    private DbSet<T> GetMockItemSet<T>(IQueryable<T> items) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(items.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(items.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(items.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(items.GetEnumerator());

        return mockSet.Object;
    }
}

public class TestTodoContext : TodoContext
{
    public TestTodoContext() : base(new DbContextOptionsBuilder<TodoContext>().Options)
    {
    }
}
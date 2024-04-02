using TodoApp.Models;
using TodoApp.Repositories;

namespace TodoApp.Services;

public interface IStateRepository : IRepository<State, long>
{
    IEnumerable<State> GetAllStates(long boardId);
}

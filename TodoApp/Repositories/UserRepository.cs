using Microsoft.EntityFrameworkCore;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class UserRepository(AuthContext authContext) : IRepository<MyUser, string> 
{
    public IEnumerable<MyUser> GetAll()
    {
        throw new NotImplementedException();
    }

    public MyUser GetById(string id)
    {
        return authContext.Users.Where(u => u.Id == id)
            .Include(u => u.Roles)
            .Single() ?? throw new ResourceNotFoundException($"User with id {id} not found.");
    }

    public MyUser Create(MyUser entity)
    {
        throw new NotImplementedException();
    }

    public MyUser Update(MyUser entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(string id)
    {
        throw new NotImplementedException();
    }
}
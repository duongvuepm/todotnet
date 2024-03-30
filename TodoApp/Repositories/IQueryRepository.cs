namespace TodoApp.Repositories;

public interface IQueryRepository<TType>
{
    IQueryable<TType> Query(Func<IQueryable<TType>, IQueryable<TType>> query)
    {
        throw new NotImplementedException();
    }
}
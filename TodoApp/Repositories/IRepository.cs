﻿using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Repositories;

public interface IRepository<TType, TId>
{
    IEnumerable<TType> GetAll();
    TType GetById(TId id);
    TType Create(TType entity);
    TType Update(TType entity);
    void Delete(TId id);

    Task<IEnumerable<TType>> GetAllAsync()
    {
        return Task.Factory.StartNew(GetAll);
    }

    Task<TType> GetByIdAsync(TId id)
    {
        return Task.Run(() => GetById(id));
    }

    IQueryable<TType> Query(Func<IQueryable<TType>, IQueryable<TType>> query);
}
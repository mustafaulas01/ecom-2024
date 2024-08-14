using System;
using System.Linq.Expressions;
using System.Resources;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public bool Exists(int id)
    {
        //generic olmasına ragmen id gelmesinin sebebi BaseEntity 'dir onda tek property id var çünkü
       return context.Set<T>().Any(x=>x.Id==id);
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null)
    {
          var query=context.Set<T>().AsQueryable();
          if(filter!=null)
             query=  query.Where(filter);
      
            return  query;

    }

    public async Task<T?> GetByIdAsync(int id)
    {
       return await context.Set<T>().FindAsync(id);
    }


    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
       return await context.Set<T>().ToListAsync();
    }



    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
       return await context.SaveChangesAsync()>0;
    }

    public void Update(T entity)
    {
      context.Set<T>().Attach(entity);
      context.Entry(entity).State=EntityState.Modified;
    }

}

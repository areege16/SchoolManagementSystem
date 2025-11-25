using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Domain.Models.Base;
using SchoolManagementSystem.Domain.RepositoryContract;
using SchoolManagementSystem.Infrastructure.Context;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Web.RepositoryImplementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    
    { 
        private readonly ApplicationContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public void Add(T item)
        {
            context.Add(item);
        }
        public void AddRange(IEnumerable<T> items)
        {
            context.AddRange(items);
        }
        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public T GetByID(int id)
        {
            return dbSet.Find(id);
        }
        public IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, bool tracked = false)
        {
            var query = dbSet.Where(expression);
            return tracked ? query : query.AsNoTracking();
        }
        public void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<T> items)
        {
            context.UpdateRange(items);
        }
        public void Remove(int id)
        {
            T entity = GetByID(id);
            if (entity != null)
            {
                context.Remove(entity);
            }
        }
        public void RemoveRange(IEnumerable<T> items)
        {
            context.RemoveRange(items);
        }   
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
   
    }
}

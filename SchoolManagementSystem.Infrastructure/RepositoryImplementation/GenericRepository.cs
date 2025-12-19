using Microsoft.EntityFrameworkCore;
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
            dbSet.Add(item);
        }
        public void AddRange(IEnumerable<T> items)
        {
            dbSet.AddRange(items);
        }
        public IQueryable<T> GetAllAsNoTracking()
        {
            return dbSet.AsNoTracking();
        }
        public async Task<T?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await dbSet.FindAsync(id, cancellationToken);
        }
        public IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, bool asTracking = false)
        {
            var query = dbSet.Where(expression);
            return asTracking ? query : query.AsNoTracking();
        }
        public void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<T> items)
        {
            dbSet.UpdateRange(items);
        }
        public void Remove(int id)
        {
            var entity = dbSet.Find(id); // TODO: Revisit 
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }
        public void RemoveRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}

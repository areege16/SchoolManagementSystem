using System.Linq.Expressions;


namespace SchoolManagementSystem.Domain.RepositoryContract
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        Task<T>? FindByIdAsync(int id, CancellationToken cancellationToken = default);
        IQueryable<T> GetAllAsNoTracking();
        IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, bool asTracking);
        void Update(T item);
        void UpdateRange(IEnumerable<T> items);
        void Remove(int id);
        void RemoveRange(IEnumerable<T> items);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

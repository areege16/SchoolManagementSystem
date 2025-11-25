using SchoolManagementSystem.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.RepositoryContract
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        T GetByID(int id);
        IQueryable<T> GetAll();
        IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, bool tracked);
        void Update(T item);
        void UpdateRange(IEnumerable<T> items);
        void Remove(int id);
        void RemoveRange(IEnumerable<T> items);    
        Task SaveChangesAsync();
    }
}

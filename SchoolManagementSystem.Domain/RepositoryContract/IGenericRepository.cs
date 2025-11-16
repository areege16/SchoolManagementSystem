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
        void Update(T item);
        void Remove(int id);
        T GetByID(int id);
        IQueryable<T> GetAll();
        IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression,bool trached );
        Task SaveChangesAsync();
    }
}

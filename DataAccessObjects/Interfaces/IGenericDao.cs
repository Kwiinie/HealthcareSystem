using BusinessObjects.Commons;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Interfaces
{
    public interface IGenericDAO<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetListById(int id);
        Task<IEnumerable<T>> GetAllAsync(string includeProperties = "");

        //filtering returns first entity
        Task<T> FindAsync(Expression<Func<T, bool>> predicate, string includeProperties = "");

        //filtering returns list
        Task<IEnumerable<T>> FindAllAsync(
    Expression<Func<T, bool>> predicate,
    string includeProperties = "");

        //pagination with filter, sort, include related entity
        Task<PaginatedList<T>> GetPagedListAsync(
            Expression<Func<T, bool>> filter,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        IQueryable<T> GetFilteredQuery(Dictionary<string, object?> filters, List<string>? includes = null);
        Task AddAsync(T entity);

        //adding a list of entities
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        IQueryable<T> Query();

        //delete a list of entities
        void RemoveRange(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}

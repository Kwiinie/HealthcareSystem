using BusinessObjects.Commons;
using DataAccessObjects.DAOs;
using DataAccessObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IGenericDAO<T> _dao;
        

        public GenericRepository(IGenericDAO<T> dao)
        {
            _dao = dao ?? throw new ArgumentNullException(nameof(dao));
            
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dao.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetListById(int id)
        {
            return await _dao.GetListById(id);
        }   

        public async Task<IEnumerable<T>> GetAllAsync(string includeProperties = "")
        {
            return await _dao.GetAllAsync(includeProperties);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            return await _dao.FindAsync(predicate, includeProperties);
        }

        public async Task<IEnumerable<T>> FindAllAsync(
                                    Expression<Func<T, bool>> predicate,
                                    string includeProperties = "")
        {
            return await _dao.FindAllAsync(predicate, includeProperties);
        }

        public async Task<PaginatedList<T>> GetPagedListAsync(
            Expression<Func<T, bool>> filter,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            return await _dao.GetPagedListAsync(filter, pageIndex, pageSize, orderBy, includeProperties);
        }

        public async Task<IEnumerable<T>> SearchAsync(Dictionary<string, object?> filters, List<string>? includes = null)
        {
            var query = _dao.GetFilteredQuery(filters, includes);
            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dao.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dao.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dao.Update(entity);
        }

        public void Remove(T entity)
        {
            _dao.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dao.RemoveRange(entities);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return await _dao.CountAsync(predicate);
        }



    }
}

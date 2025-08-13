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
    public interface IProfessionalDao : IGenericDAO<Professional>
    {
        Task<Professional> GetByIdAsync(int id);
        Task<PaginatedList<Professional>> GetAllProfessionalsPagedAsync(
            Expression<Func<Professional, bool>> filter,
            int pageIndex,
            int pageSize,
            Func<IQueryable<Professional>, IOrderedQueryable<Professional>> orderBy = null);
    }
}

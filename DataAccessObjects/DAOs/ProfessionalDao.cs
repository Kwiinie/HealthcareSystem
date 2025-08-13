using BusinessObjects.Commons;
using BusinessObjects.Entities;
using DataAccessObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DAOs
{
    public class ProfessionalDao : GenericDAO<Professional>, IProfessionalDao
    {
        private readonly FindingHealthcareSystemContext _context;

        public ProfessionalDao(FindingHealthcareSystemContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Professional> GetByIdAsync(int id)
        {
            return await _context.Professionals
               .Include(f => f.Expertise)
               .Include(f => f.User)
               .Include(f => f.PrivateServices)
               .Include(f => f.ProfessionalSpecialties)
               .ThenInclude(f => f.Specialty)
               .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Professional> GetByUserIdAsync(int userId)
        {
            return await _context.Professionals
               .Include(f => f.Expertise)
               .Include(f => f.User)
               .Include(f => f.PrivateServices)
               .Include(f => f.ProfessionalSpecialties)
               .ThenInclude(f => f.Specialty)
               .FirstOrDefaultAsync(f => f.UserId == userId);
        }

        public async Task<PaginatedList<Professional>> GetAllProfessionalsPagedAsync(
    Expression<Func<Professional, bool>> filter,
    int pageIndex,
    int pageSize,
    Func<IQueryable<Professional>, IOrderedQueryable<Professional>> orderBy = null)
        {
            IQueryable<Professional> query = _context.Professionals
                .Include(p => p.Expertise)
                .Include(p => p.User)
                .Include(p => p.PrivateServices)
                .Include(p => p.ProfessionalSpecialties)
                .ThenInclude(ps => ps.Specialty);

            // Apply filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply ordering
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await PaginatedList<Professional>.CreateAsync(query, pageIndex, pageSize);
        }

        

    }
}

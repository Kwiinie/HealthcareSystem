using BusinessObjects.Commons;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
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
    public class ProfessionalRepository : IProfessionalRepository
    {
        private readonly IProfessionalDao _professionalDao;

        public ProfessionalRepository(IGenericDAO<Professional> dao, IProfessionalDao professionalDao)
        {
            _professionalDao = professionalDao;
        }

        public async Task<Professional> GetByIdAsync(int id)
        {
            return await _professionalDao.GetByIdAsync(id);
        }

        


        /// <summary>
        /// SEARCHING PROFESSIONAL BASED ON LOCATION AND NAME
        /// </summary>
        /// <param name="province"></param>
        /// <param name="district"></param>
        /// <param name="ward"></param>
        /// <param name="specialty"></param>
        /// <param name="professionalName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Professional>> SearchAsync( string? province = null,
                                                                  string? district = null,
                                                                  string? ward = null,
                                                                  string? specialty = null,
                                                                  string? professionalName = null) 
        {
            var filters = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(province))
            {
                filters.Add("Province", province);
            }

            if (!string.IsNullOrEmpty(district))
            {
                filters.Add("District", district);
            }

            if (!string.IsNullOrEmpty(ward))
            {
                filters.Add("Ward", ward);
            }

            if (!string.IsNullOrEmpty(professionalName))
            {
                filters.Add("User.Name", professionalName);
            }

            filters.Add("RequestStatus", ProfessionalRequestStatus.Approved);

            var query = _professionalDao.GetFilteredQuery(filters, includes: new List<string> {
            "Expertise",
            "PrivateServices",
            "ProfessionalSpecialties",
            "ProfessionalSpecialties.Specialty",
            "User" 
            });

            if (!string.IsNullOrEmpty(specialty))
            {
                query = query.Where(professional =>
                    professional.ProfessionalSpecialties.Any(ps =>
                        ps.Specialty.Name != null &&
                        ps.Specialty.Name.Contains(specialty)
                    )
                );
            }

            if (!string.IsNullOrEmpty(professionalName))
            {
                query = query.Where(professional =>
                    professional.User.Fullname.Contains(professionalName));
            }


            return await query.ToListAsync();
        }

        public async Task<PaginatedList<Professional>> GetAllProfessionalsPagedAsync(
            Expression<Func<Professional, bool>> filter,
            int pageIndex,
            int pageSize,
            Func<IQueryable<Professional>, IOrderedQueryable<Professional>> orderBy = null)
        {
            return await _professionalDao.GetAllProfessionalsPagedAsync(filter, pageIndex, pageSize, orderBy);
        }

    }
}

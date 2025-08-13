using Repositories.Interfaces;
using DataAccessObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Enums;

namespace Repositories.Repositories
{
    public class FacilityRepository : GenericRepository<Facility>, IFacilityRepository
    {
        private readonly IFacilityDao _facilityDao;
        private readonly IGenericDAO<Facility> _dao;


        public FacilityRepository(IFacilityDao facilityDao, IGenericDAO<Facility> dao) : base(dao)
        {
            _facilityDao = facilityDao;
            _dao = dao;

        }

        public async Task<Facility> GetByIdWithRelationsAsync(int id)
        {
            return await _facilityDao.GetByIdWithRelationsAsync(id);
        }

        public async Task<Facility> CreateAsync(Facility facility)
        {
            return await _facilityDao.CreateAsync(facility);
        }

        public async Task CreateFacilityDepartmentsAsync(List<FacilityDepartment> facilityDepartments)
        {
            await _facilityDao.CreateFacilityDepartmentsAsync(facilityDepartments);
        }

        public async Task UpdateFacilityDepartmentsAsync(int facilityId, List<int> departmentIds)
        {
            await _facilityDao.UpdateFacilityDepartmentsAsync(facilityId, departmentIds);
        }

        public async Task<IEnumerable<Facility>> SearchAsync(string? name = null,
                                                              string? province = null,
                                                              string? district = null,
                                                              string? ward = null,
                                                              string? department = null)
        {
            var filters = new Dictionary<string, object?>();

            if (!string.IsNullOrEmpty(name))
            {
                filters.Add("Name", name);
            }

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

            filters.Add("Status", FacilityStatus.Active);


            var query = _dao.GetFilteredQuery(filters, includes: new List<string>
            {
            "Type",
            "FacilityDepartments",
            "FacilityDepartments.Department",
            "PublicServices"
            });

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(Facility =>
                    Facility.FacilityDepartments.Any(ps =>
                        ps.Department.Name != null &&
                        ps.Department.Name.Contains(department) && Facility.Status == BusinessObjects.Enums.FacilityStatus.Active
                    )
                );
            }

            return await query.ToListAsync();
        }
    }
}

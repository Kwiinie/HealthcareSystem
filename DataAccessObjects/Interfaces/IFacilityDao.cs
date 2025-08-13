using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Interfaces
{
    public interface IFacilityDao
    {
        Task<Facility> GetByIdWithRelationsAsync(int id);

        Task<Facility> CreateAsync(Facility facility);

        Task CreateFacilityDepartmentsAsync(List<FacilityDepartment> facilityDepartments);

        Task UpdateFacilityDepartmentsAsync(int facilityId, List<int> departmentIds);
    }
}

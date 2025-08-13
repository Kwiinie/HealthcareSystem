using BusinessObjects.Entities;

namespace Repositories.Interfaces
{
    public interface IFacilityRepository
    {
        Task<Facility> GetByIdWithRelationsAsync(int id);

        Task<Facility> CreateAsync(Facility facility);

        Task CreateFacilityDepartmentsAsync(List<FacilityDepartment> facilityDepartments);

        Task UpdateFacilityDepartmentsAsync(int facilityId, List<int> departmentIds);
        Task<IEnumerable<Facility>> SearchAsync(string? name = null,
                                                string? province = null,
                                                string? district = null,
                                                string? ward = null,
                                                string? department = null);
    }
}
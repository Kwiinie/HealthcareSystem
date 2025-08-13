using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.Entities;

namespace Services.Interfaces
{
    public interface IFacilityService
    {
        Task<List<FacilityDto>> GetAllFacilities();
        Task<IEnumerable<FacilityDto>> SearchFacilities(string? name, string? province, string? operationDay, string? district, string? ward, bool isAdmin, int? typeId);
        Task<FacilityDto> Create(FacilityDto facilityDto);
        Task<FacilityDto> Update(int id, FacilityDto facilityDto);
        Task<FacilityDto> GetById(int id);
        Task<SearchingFacilityDto> GetFacilityById(int id);

        Task<FacilityDto> DeleteAsync(int id);

        Task<IEnumerable<SearchingFacilityDto>> SearchAsync(string? name = null,
                                                            string? province = null,
                                                            string? district = null,
                                                            string? ward = null,
                                                            string? department = null);
    }
}
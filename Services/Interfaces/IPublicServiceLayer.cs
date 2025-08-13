using BusinessObjects.DTOs.Service;

namespace Services.Interfaces
{
    public interface IPublicServiceLayer
    {
        Task<List<ServiceDto>> GetAllFacilities();
        Task<ServiceDto> GetPublicServiceById(int publicServiceId);
        Task<List<ServiceDto>> GetServicesByFacilityId(int facilityId);
        Task<ServiceDto> Create(int facilityId, ServiceDto publicServiceDto);

        Task Delete(int publicServiceId );

        Task<ServiceDto> Update(int facilityId, int publicServiceId, ServiceDto publicServiceDto);
    }
}
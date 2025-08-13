using BusinessObjects.DTOs.Facility;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFacilityTypeService
    {
        Task<List<FacilityTypeDto>> GetAllFacilityTypes();
        Task<FacilityTypeDto> Create(FacilityTypeDto facilityDto);
        Task<FacilityTypeDto> Update(int id, FacilityTypeDto facilityDto);
        Task<FacilityTypeDto> GetById(int id);
        Task<FacilityTypeDto> DeleteAsync(int id);
        Task<List<FacilityTypeDto>> GetAllActiveFacilityTypes();
    }
}

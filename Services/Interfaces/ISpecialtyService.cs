using BusinessObjects.DTOs.Department;
using BusinessObjects.DTOs.Professional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISpecialtyService
    {
        Task<List<SpecialtyDto>> GetAllSpecialties();
        Task<SpecialtyDto> GetSpecialtyById(int id);
        Task<SpecialtyDto> CreateSpecialty(SpecialtyDto input);
        Task<SpecialtyDto> UpdateSpecialty(SpecialtyDto input);
        Task DeleteSpecialty(int id);
    }
}

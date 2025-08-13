using BusinessObjects.Commons;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs;
using BusinessObjects.DTOs.User;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<PaginatedList<GeneralUserDto>> GetUsersAsync(
        string? search,
        string? role,
        string? status,
        string? sortBy,
        bool isDescending,
        int pageIndex,
        int pageSize);

        Task<GeneralUserDto?> GetUserByIdAsync(int userId);
        Task AddUserAsync(GeneralUserDto userDto);
        Task UpdateUserAsync(GeneralUserDto userDto);
        Task DeleteUserAsync(int id);
        Task<User> GetUserByIdNew(int userId);
        Task<Professional> GetProfessionalById(int userId);

        Task<List<Specialty>> GetAllSpecialtiesAsync();
        Task<List<FacilityDepartment>> GetAllHospitalsAsync();

        Task<List<Expertise>> GetAllExpertises();
        Task UpdateUserStatus(GeneralUserDto userDto);
        Task RegisterUserAsync(RegisterUserDto userDto);

        Task UpdateProfessionalAsync(Professional professional);
        Task UpdatePatientAsync(Patient patient);
        Task<Patient> GetPatientById(int userId);
        Task<IEnumerable<PatientDTO>> GetAllPatientAsync();
        Task UploadUserImageAsync(int userId, string imgUrl);

    }
}

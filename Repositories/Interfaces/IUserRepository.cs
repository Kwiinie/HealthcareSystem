using BusinessObjects.DTOs.User;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);


        Task<List<Specialty>> GetAllSpecialtiesAsync();
        Task<List<FacilityDepartment>> GetAllHospitalsAsync();
        Task<Professional> GetProfessionalById(int userId);
        Task<Patient> GetPatientById(int userId);

        Task<List<Expertise>> GetAllExpertises();

        Task RegisterUserAsync(RegisterUserDto userDto);
        Task<bool> EmailExistsAsync(string email);
        Task UpdateProfessionalAsync(Professional professional);
        Task UpdatePatientAsync(Patient patient);
        Task<IEnumerable<Professional>> FindAllWithProfessionalAsync(Expression<Func<Professional, bool>> predicate);
        Task<IEnumerable<Patient>> FindAllWithPatientAsync();

    }
}

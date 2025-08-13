using AutoMapper;
using BusinessObjects.Commons;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs;
using BusinessObjects.DTOs.User;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper , IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<PaginatedList<GeneralUserDto>> GetUsersAsync(
            string? search,
            string? role,
            string? status,
            string? sortBy,
            bool isDescending,
            int pageIndex,
            int pageSize)
        {
            var userRepo = _unitOfWork.GetRepository<User>();

            Expression<Func<User, bool>> filter = u =>
                (string.IsNullOrEmpty(search) || u.Fullname.Contains(search) || u.Email.Contains(search) || u.PhoneNumber.Contains(search)) &&
                (string.IsNullOrEmpty(role) || u.Role.ToString() == role) &&
                (string.IsNullOrEmpty(status) || u.Status.ToString() == status);

            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null;

            if (!string.IsNullOrEmpty(sortBy))
            {
                orderBy = query =>
                {
                    return isDescending ? query.OrderByDescending(GetSortProperty(sortBy)) : query.OrderBy(GetSortProperty(sortBy));
                };
            }

            var paginatedUsers = await userRepo.GetPagedListAsync(filter, pageIndex, pageSize, orderBy);

            return new PaginatedList<GeneralUserDto>(
                paginatedUsers.Select(_mapper.Map<GeneralUserDto>).ToList(),
                paginatedUsers.Count,
                pageIndex,
                pageSize);
        }

        public async Task<GeneralUserDto?> GetUserByIdAsync(int userId)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.GetByIdAsync(userId);
            return user == null ? null : _mapper.Map<GeneralUserDto>(user);
        }

        public async Task<User> GetUserByIdNew(int userId)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.GetByIdAsync(userId);
            return user;
        }

        private static Expression<Func<User, object>> GetSortProperty(string sortBy)
        {
            return sortBy.ToLower() switch
            {
                "fullname" => u => u.Fullname,
                "email" => u => u.Email,
                "phonenumber" => u => u.PhoneNumber,
                "role" => u => u.Role,
                "status" => u => u.Status,
                _ => u => u.Id
            };
        }

        public async Task AddUserAsync(GeneralUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.GetRepository<User>().AddAsync(user);
        }

        public async Task UpdateUserAsync(GeneralUserDto userDto)
        {
            try
            {
                // Validate phone number (must be exactly 10 digits)
                if (string.IsNullOrEmpty(userDto.PhoneNumber) || !Regex.IsMatch(userDto.PhoneNumber, @"^\d{10}$"))
                {
                    throw new Exception("Phone number must be exactly 10 digits.");
                }

              
                var userRepo = _unitOfWork.GetRepository<User>();
                var user = await userRepo.GetByIdAsync(userDto.Id);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                _mapper.Map(userDto, user); // Update properties from DTO

                userRepo.Update(user); // No need to await since it's void
                await _unitOfWork.SaveChangesAsync(); // Ensure changes are persisted
            }
            catch(Exception ex)
            {
                throw ex;
            }
          
        }
        public async Task UpdateUserStatus(GeneralUserDto userDto)
        {
            try
            {
               


                var userRepo = _unitOfWork.GetRepository<User>();
                var user = await userRepo.GetByIdAsync(userDto.Id);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                _mapper.Map(userDto, user); // Update properties from DTO

                userRepo.Update(user); // No need to await since it's void
                await _unitOfWork.SaveChangesAsync(); // Ensure changes are persisted
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task UploadUserImageAsync(int userId, string imgUrl)
        {
            try
            {
                var userRepo = _unitOfWork.GetRepository<User>();
                var user = await userRepo.GetByIdAsync(userId);

                if (user == null)
                {
                    throw new Exception("Không tìm thấy người dùng.");
                }

                user.ImgUrl = imgUrl;

                userRepo.Update(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật ảnh người dùng: {ex.Message}", ex);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.GetByIdAsync(id);
            user.IsDeleted = true;
           
            await _unitOfWork.SaveChangesAsync(); // Ensure changes are persisted

        }

        public async Task<List<Specialty>> GetAllSpecialtiesAsync()
        {
            return await _userRepository.GetAllSpecialtiesAsync();
        }

        public async Task<List<FacilityDepartment>> GetAllHospitalsAsync()
        {
            return await _userRepository.GetAllHospitalsAsync();
        }

        public async Task RegisterUserAsync(RegisterUserDto userDto)
        {
            var date = userDto.Birthday;
            try
            {
                // Validate phone number (must be exactly 10 digits)
                if (string.IsNullOrEmpty(userDto.PhoneNumber) || !Regex.IsMatch(userDto.PhoneNumber, @"^\d{10}$"))
                {
                    throw new Exception("Phone number must be exactly 10 digits.");
                }

                // Kiểm tra email đã tồn tại chưa
                if (await _userRepository.EmailExistsAsync(userDto.Email))
                {
                    throw new Exception("Email đã tồn tại. Vui lòng sử dụng email khác.");
                }
                await _userRepository.RegisterUserAsync(userDto);

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        // Kiểm tra email đã tồn tại chưa
        //bool emailExists = await _context.Users.AnyAsync(p => p.Email == userDto.Email);
        //if (emailExists)
        //{
        //    throw new Exception("Email đã tồn tại. Vui lòng sử dụng email khác.");
        //}

        public async Task<List<Expertise>> GetAllExpertises()
        {
            return await _userRepository.GetAllExpertises();
        }

        public async Task<Professional> GetProfessionalById(int userId)
        {
            return await _userRepository.GetProfessionalById(userId);


        }

        public async Task UpdateProfessionalAsync(Professional professional)
        {

            await _userRepository.UpdateProfessionalAsync(professional);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _userRepository.UpdatePatientAsync(patient);
        }

        public async Task<Patient> GetPatientById(int userId)
        {
            return await _userRepository.GetPatientById(userId);
        }

        public async Task<IEnumerable<PatientDTO>> GetAllPatientAsync()
        {
            var patients = await _unitOfWork.UserRepository.FindAllWithPatientAsync();
            if (patients == null)
            {
                return new List<PatientDTO>();
            }
            return _mapper.Map<IEnumerable<PatientDTO>>(patients);
        }
    }
}

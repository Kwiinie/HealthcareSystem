using AutoMapper;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Department;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<DepartmentDto>> GetAllDepartments()
        {
            var departmentRepo = _unitOfWork.GetRepository<Department>();
            var departments = await departmentRepo.GetAllAsync();
            if (departments == null || !departments.Any())
            {
                return new List<DepartmentDto>();
            }
            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> Create(DepartmentDto departmentDto)
        {
            if (string.IsNullOrEmpty(departmentDto.Name))
            {
                throw new Exception("Department name is required");
            }
            if (string.IsNullOrEmpty(departmentDto.Description))
            {
                throw new Exception("Department description is required");
            }
            var departmentRepo = _unitOfWork.GetRepository<Department>();
            var department = _mapper.Map<Department>(departmentDto);
            department.CreatedAt = DateTime.UtcNow;
            await departmentRepo.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> Update(int id, DepartmentDto departmentDto)
        {
            if (string.IsNullOrEmpty(departmentDto.Name))
            {
                throw new Exception("Department name is required");
            }
            if (string.IsNullOrEmpty(departmentDto.Description))
            {
                throw new Exception("Department description is required");
            }
            var departmentRepo = _unitOfWork.GetRepository<Department>();
            var department = await departmentRepo.GetByIdAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
            department.Name = departmentDto.Name;
            department.Description = departmentDto.Description;
            department.UpdatedAt = DateTime.UtcNow;
            departmentRepo.Update(department);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> GetById(int id)
        {
            var departmentRepo = _unitOfWork.GetRepository<Department>();
            var department = await departmentRepo.GetByIdAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<List<DepartmentDto>> GetDepartmentsByFacilityIdAsync(int facilityId)
        {
            var departmentRepo = _unitOfWork.GetRepository<Department>();
            var departments = await departmentRepo.FindAllAsync(d => d.Id == facilityId);
            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task Delete(int id)
        {
            var departmentRepo = _unitOfWork.GetRepository<Department>();
            var department = await departmentRepo.GetByIdAsync(id);
            if (department == null)
            {
                throw new Exception("Department not found");
            }
            department.IsDeleted = true;
            department.UpdatedAt = DateTime.UtcNow;
            departmentRepo.Update(department);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

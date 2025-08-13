using AutoMapper;
using BusinessObjects.DTOs.Department;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecialtyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SpecialtyDto>> GetAllSpecialties()
        {
            var specialtyRepo = _unitOfWork.GetRepository<Specialty>();
            var specialties = await specialtyRepo.GetAllAsync();
            return _mapper.Map<List<SpecialtyDto>>(specialties);
        }

        public async Task<SpecialtyDto> GetSpecialtyById(int id)
        {
            var specialtyRepo = _unitOfWork.GetRepository<Specialty>();
            var specialty = await specialtyRepo.GetByIdAsync(id);

            if (specialty == null || specialty.IsDeleted)
            {
                throw new Exception("Không tìm thấy chuyên khoa.");
            }

            return _mapper.Map<SpecialtyDto>(specialty);
        }

        public async Task<SpecialtyDto> CreateSpecialty(SpecialtyDto input)
        {
            var specialtyRepo = _unitOfWork.GetRepository<Specialty>();

            // Check if a specialty with same name exists
            var existingSpecialty = await specialtyRepo.FindAsync(x => x.Name == input.Name && !x.IsDeleted);
            if (existingSpecialty != null)
            {
                throw new Exception("Chuyên khoa với tên này đã tồn tại.");
            }

            var newSpecialty = _mapper.Map<Specialty>(input);
            newSpecialty.CreatedAt = DateTime.Now;
            newSpecialty.UpdatedAt = DateTime.Now;
            newSpecialty.IsDeleted = false;

            await specialtyRepo.AddAsync(newSpecialty);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SpecialtyDto>(newSpecialty);
        }

        public async Task<SpecialtyDto> UpdateSpecialty(SpecialtyDto input)
        {
            if (!input.Id.HasValue)
            {
                throw new Exception("ID chuyên khoa không được để trống.");
            }

            var specialtyRepo = _unitOfWork.GetRepository<Specialty>();

            // Check if specialty exists
            var existingSpecialty = await specialtyRepo.GetByIdAsync(input.Id.Value);
            if (existingSpecialty == null || existingSpecialty.IsDeleted)
            {
                throw new Exception("Không tìm thấy chuyên khoa.");
            }

            // Check if a different specialty with same name exists
            var duplicateSpecialty = await specialtyRepo.FindAsync(x => x.Name == input.Name && x.Id != input.Id && !x.IsDeleted);

            if (duplicateSpecialty != null)
            {
                throw new Exception("Chuyên khoa với tên này đã tồn tại.");
            }

            existingSpecialty.Name = input.Name;
            existingSpecialty.Description = input.Description;
            existingSpecialty.UpdatedAt = DateTime.Now;

            specialtyRepo.Update(existingSpecialty);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SpecialtyDto>(existingSpecialty);
        }

        public async Task DeleteSpecialty(int id)
        {
            var specialtyRepo = _unitOfWork.GetRepository<Specialty>();

            var specialty = await specialtyRepo.GetByIdAsync(id);
            if (specialty == null || specialty.IsDeleted)
            {
                throw new Exception("Không tìm thấy chuyên khoa.");
            }

            // Soft delete
            specialty.IsDeleted = true;
            specialty.UpdatedAt = DateTime.Now;

            specialtyRepo.Update(specialty);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

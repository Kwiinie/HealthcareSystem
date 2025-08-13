using AutoMapper;
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
    public class FacilityTypeService : IFacilityTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FacilityTypeService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<FacilityTypeDto>> GetAllFacilityTypes()
        {
            var facRepo = _unitOfWork.GetRepository<FacilityType>();
            var facilities = await facRepo.GetAllAsync();
            if (facilities == null || !facilities.Any())
            {
                return new List<FacilityTypeDto>();
            }
            return _mapper.Map<List<FacilityTypeDto>>(facilities);
        }

        public async Task<List<FacilityTypeDto>> GetAllActiveFacilityTypes()
        {
            var facRepo = _unitOfWork.GetRepository<FacilityType>();
            var facilities = await facRepo.GetAllAsync();
            if (facilities == null || !facilities.Any())
            {
                return new List<FacilityTypeDto>();
            }
            var result = facilities.Where( fac => fac.IsDeleted = true ).ToList();
            return _mapper.Map<List<FacilityTypeDto>>(result);
        }

        public async Task<FacilityTypeDto> Create (FacilityTypeDto facilityTypeDto)
        {
            if (string.IsNullOrEmpty(facilityTypeDto.Name))
            {
                throw new Exception("Facility Type name is required");
            }
            if (string.IsNullOrEmpty(facilityTypeDto.Description))
            {
                throw new Exception("Facility Type description is required");
            }
            var facRepo = _unitOfWork.GetRepository<FacilityType>();
            var facility = _mapper.Map<FacilityType>(facilityTypeDto);
            await facRepo.AddAsync(facility);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<FacilityTypeDto>(facility);
        }

        public async Task<FacilityTypeDto> Update(int id, FacilityTypeDto facilityTypeDto)
        {
            if (string.IsNullOrEmpty(facilityTypeDto.Name))
            {
                throw new Exception("Facility Type name is required");
            }
            if (string.IsNullOrEmpty(facilityTypeDto.Description))
            {
                throw new Exception("Facility Type description is required");
            }
            var facRepo = _unitOfWork.GetRepository<FacilityType>();
            var facility = await facRepo.GetByIdAsync(id);
            if (facility == null)
            {
                throw new Exception("Facility Type not found");
            }
            facility.Name = facilityTypeDto.Name;
            facility.Description = facilityTypeDto.Description;
            facRepo.Update(facility);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<FacilityTypeDto>(facility);
        }

        public async Task<FacilityTypeDto> GetById(int id)
        {
            var facRepo = _unitOfWork.GetRepository<FacilityType>();
            var facility = await facRepo.GetByIdAsync(id);
            if (facility == null)
            {
                throw new Exception("Facility Type not found");
            }
            return _mapper.Map<FacilityTypeDto>(facility);
        }

        public async Task<FacilityTypeDto> DeleteAsync(int id)
        {
            var facRepo = _unitOfWork.GetRepository<FacilityType>();
            var facility = await facRepo.GetByIdAsync(id);
            if (facility == null)
            {
                throw new Exception("Facility Type not found");
            }
            facility.IsDeleted = true;
            facility.UpdatedAt = DateTime.UtcNow.AddHours(7);
            facRepo.Update(facility);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<FacilityTypeDto>(facility);
        }
    }
}

using AutoMapper;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PublicServiceLayer : IPublicServiceLayer
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublicServiceLayer(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ServiceDto>> GetAllFacilities()
        {
            var facRepo = _unitOfWork.GetRepository<PublicService>();
            var facilities = await facRepo.GetAllAsync();
            if (facilities == null || !facilities.Any())
            {
                return new List<ServiceDto>();
            }
            return _mapper.Map<List<ServiceDto>>(facilities);
        }

        public async Task<ServiceDto> GetPublicServiceById(int publicServiceId)
        {
            var pubServiceRepo = _unitOfWork.GetRepository<PublicService>();
            var pubService = await pubServiceRepo.GetByIdAsync(publicServiceId);
            if (pubService == null)
            {
                throw new Exception("Public Service not found");
            }
            return _mapper.Map<ServiceDto>(pubService);
        }

        public async Task<List<ServiceDto>> GetServicesByFacilityId(int facilityId)
        {
            var pubServiceRepo = _unitOfWork.GetRepository<PublicService>();
            var pubServices = await pubServiceRepo.FindAllAsync(x => x.FacilityId == facilityId);
            if (pubServices == null || !pubServices.Any())
            {
                return new List<ServiceDto>();
            }
            return _mapper.Map<List<ServiceDto>>(pubServices);
        }

        public async Task<ServiceDto> Create(int facilityId, ServiceDto publicServiceDto)
        {
            if (string.IsNullOrEmpty(publicServiceDto.Name))
            {
                throw new Exception("Public Service name is required");
            }
            if (string.IsNullOrEmpty(publicServiceDto.Description))
            {
                throw new Exception("Public Service description is required");
            }
            var facRepo = _unitOfWork.GetRepository<Facility>();
            var existedFacility = await facRepo.GetByIdAsync(facilityId);
            if (existedFacility == null)
            {
                throw new Exception("Facility is not found");
            }
            var pubServiceRepo = _unitOfWork.GetRepository<PublicService>();
            var pubService = _mapper.Map<PublicService>(publicServiceDto);
            pubService.FacilityId = facilityId;
            pubService.CreatedAt = DateTime.UtcNow;
            await pubServiceRepo.AddAsync(pubService);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ServiceDto>(pubService);
        }

        public async Task<ServiceDto> Update(int facilityId, int publicServiceId, ServiceDto publicServiceDto)
        {
            if (string.IsNullOrEmpty(publicServiceDto.Name))
            {
                throw new Exception("Public Service name is required");
            }
            if (string.IsNullOrEmpty(publicServiceDto.Description))
            {
                throw new Exception("Public Service description is required");
            }
            var facRepo = _unitOfWork.GetRepository<Facility>();
            var existedFacility = await facRepo.GetByIdAsync(facilityId);
            if (existedFacility == null)
            {
                throw new Exception("Facility is not found");
            }
            var pubServiceRepo = _unitOfWork.GetRepository<PublicService>();
            var pubService = await pubServiceRepo.GetByIdAsync(publicServiceId);
            if (pubService == null)
            {
                throw new Exception("Public Service not found");
            }
            pubService.Name = publicServiceDto.Name;
            pubService.Description = publicServiceDto.Description;
            pubService.Price = publicServiceDto.Price;
            pubServiceRepo.Update(pubService);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ServiceDto>(pubService);
        }

        public async Task Delete(int publicServiceId)
        {
        
            var pubServiceRepo = _unitOfWork.GetRepository<PublicService>();
            var pubService = await pubServiceRepo.GetByIdAsync(publicServiceId);
            if (pubService == null)
            {
                throw new Exception("Public Service not found");
            }
            pubService.IsDeleted = true;
            pubServiceRepo.Update(pubService);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

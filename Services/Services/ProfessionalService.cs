using AutoMapper;
using BusinessObjects.Commons;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProfessionalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// GET PROFESSIONAL BY ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProfessionalDto> GetById(int id)
        {
            var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(id);
            return _mapper.Map<ProfessionalDto>(professional);
        }



        /// <summary>
        /// SEARCHING PROFESSIONALS BASED ON LOCATION, SPECIALTY AND NAME
        /// </summary>
        /// <param name="province"></param>
        /// <param name="district"></param>
        /// <param name="ward"></param>
        /// <param name="specialty"></param>
        /// <param name="professionalName"></param>
        /// <returns>LIST PROFESSIONALS WITH USER INFO, SERVICE INFO, LIST SPECIALTY AND EXPERTISE</returns>
        public async Task<IEnumerable<ProfessionalDto>> SearchAsync(string? province,
                                                                    string? district,
                                                                    string? ward,
                                                                    string? specialty,
                                                                    string? professionalName)
        {

            var professionals = await _unitOfWork.ProfessionalRepository.SearchAsync(
             province, district, ward, specialty, professionalName);

            var professionalDtos = _mapper.Map<IEnumerable<ProfessionalDto>>(professionals);

            return professionalDtos;
        }

        public async Task<PaginatedList<ProfessionalDto>> GetProfessionalsPagedAsync(
        Expression<Func<Professional, bool>> filter = null,
        int pageIndex = 1,
        int pageSize = 10,
        Func<IQueryable<Professional>, IOrderedQueryable<Professional>> orderBy = null)
        {
            var result = await _unitOfWork.ProfessionalRepository.GetAllProfessionalsPagedAsync(
                filter,
                pageIndex,
                pageSize,
                orderBy ?? (q => q.OrderBy(p => p.Id)) // Default sort by Id
            );
            return _mapper.Map<PaginatedList<ProfessionalDto>>(result);
        }

        public async Task<IEnumerable<ProfessionalDto>> GetAllProfessionalAsync(ProfessionalRequestStatus requestStatus)
        {
            var professionals = await _unitOfWork.UserRepository.FindAllWithProfessionalAsync(u => u.RequestStatus == requestStatus);
            if (professionals == null)
            {
                return new List<ProfessionalDto>();
            }
            return _mapper.Map<IEnumerable<ProfessionalDto>>(professionals);
        }
        public async Task<List<ServiceDto>> GetServicesByprofessionalID(int professionalID)
        {
            var pubServiceRepo = _unitOfWork.GetRepository<PrivateService>();
            var pubServices = await pubServiceRepo.FindAllAsync(x => x.ProfessionalId == professionalID);
            if (pubServices == null || !pubServices.Any())
            {
                return new List<ServiceDto>();
            }
            return _mapper.Map<List<ServiceDto>>(pubServices);
        }
        public async Task<List<ServiceDto>> GetServicesByProId(int professId)
        {
            var pubServiceRepo = _unitOfWork.GetRepository<PrivateService>();
            var pubServices = await pubServiceRepo.FindAllAsync(x => x.ProfessionalId == professId);
            if (pubServices == null || !pubServices.Any())
            {
                return new List<ServiceDto>();
            }
            return _mapper.Map<List<ServiceDto>>(pubServices);
        }

        public async Task<Professional> GetProfessionalByProId(int professId) {
             
            return await  _unitOfWork.ProfessionalRepository.GetByIdAsync(professId);
           
        }


        public async Task<ServiceDto> Create(int professionalID, ServiceDto publicServiceDto)
        {
            if (string.IsNullOrEmpty(publicServiceDto.Name))
            {
                throw new Exception("Private Service name is required");
            }
            if (string.IsNullOrEmpty(publicServiceDto.Description))
            {
                throw new Exception("Private Service description is required");
            }
            var facRepo = _unitOfWork.GetRepository<Professional>();
            var existedFacility = await facRepo.GetByIdAsync(professionalID);
            if (existedFacility == null)
            {
                throw new Exception("Professional is not found");
            }
            var pubServiceRepo = _unitOfWork.GetRepository<PrivateService>();
            var pubService = _mapper.Map<PrivateService>(publicServiceDto);
            pubService.ProfessionalId = professionalID;
            pubService.CreatedAt = DateTime.UtcNow;
            await pubServiceRepo.AddAsync(pubService);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ServiceDto>(pubService);
        }

        public async Task<ServiceDto> Update(int professionalID, int publicServiceId, ServiceDto publicServiceDto)
        {
            if (string.IsNullOrEmpty(publicServiceDto.Name))
            {
                throw new Exception("Private Service name is required");
            }
            if (string.IsNullOrEmpty(publicServiceDto.Description))
            {
                throw new Exception("Private Service description is required");
            }
            var facRepo = _unitOfWork.GetRepository<Professional>();
            var existedFacility = await facRepo.GetByIdAsync(professionalID);
            if (existedFacility == null)
            {
                throw new Exception("Professional is not found");
            }
            var pubServiceRepo = _unitOfWork.GetRepository<PrivateService>();
            var pubService = await pubServiceRepo.GetByIdAsync(publicServiceId);
            if (pubService == null)
            {
                throw new Exception("Private Service not found");
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

            var pubServiceRepo = _unitOfWork.GetRepository<PrivateService>();
            var pubService = await pubServiceRepo.GetByIdAsync(publicServiceId);
            if (pubService == null)
            {
                throw new Exception("Private Service not found");
            }
            pubService.IsDeleted = true;

            pubServiceRepo.Update(pubService);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ServiceDto> GetPrivateServiceById(int professionalService)
        {
            var pubServiceRepo = _unitOfWork.GetRepository<PrivateService>();
            var pubService = await pubServiceRepo.GetByIdAsync(professionalService);
            if (pubService == null)
            {
                throw new Exception("Private Service not found");
            }
            return _mapper.Map<ServiceDto>(pubService);
        }
    }
}

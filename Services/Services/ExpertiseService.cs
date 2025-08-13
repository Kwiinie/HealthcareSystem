using AutoMapper;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;

namespace Services.Services
{
    public class ExpertiseService : IExpertiseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpertiseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ExpertiseDTO>> GetAllExpertises()
        {
            var expertiseRepo = _unitOfWork.GetRepository<Expertise>();
            var expertises = await expertiseRepo.GetAllAsync();
            if (expertises == null || !expertises.Any())
            {
                return new List<ExpertiseDTO>();
            }
            return _mapper.Map<List<ExpertiseDTO>>(expertises);
        }

        public async Task<ExpertiseDTO> CreateExpertise (ExpertiseDTO expertiseDTO)
        {
            var expertiseRepo = _unitOfWork.GetRepository<Expertise>();
            var expertise = _mapper.Map<Expertise>(expertiseDTO);
            expertise.CreatedAt = DateTime.UtcNow;
            await expertiseRepo.AddAsync(expertise);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ExpertiseDTO>(expertise);
        }

        public async Task<ExpertiseDTO> UpdateExpertise(int id, ExpertiseDTO expertiseDTO)
        {
            var expertiseRepo = _unitOfWork.GetRepository<Expertise>();
            var expertise = await expertiseRepo.GetByIdAsync(id);
            if (expertise == null)
            {
                throw new Exception("Expertise not found");
            }
            expertise.Name = expertiseDTO.Name;
            expertise.Description = expertiseDTO.Description;
            expertise.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ExpertiseDTO>(expertise);
        }

        public async Task DeleteExpertise(int id)
        {
            var expertiseRepo = _unitOfWork.GetRepository<Expertise>();
            var expertise = await expertiseRepo.GetByIdAsync(id);
            if (expertise == null)
            {
                throw new Exception("Expertise not found");
            }
            expertise.IsDeleted = true;
            expertise.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ExpertiseDTO> GetById(int id)
        {
            var expertiseRepo = _unitOfWork.GetRepository<Expertise>();
            var expertise = await expertiseRepo.GetByIdAsync(id);
            if (expertise == null)
            {
                throw new Exception("Expertise not found");
            }
            return _mapper.Map<ExpertiseDTO>(expertise);
        }
    }
}

using BusinessObjects.DTOs;

namespace Services.Interfaces
{
    public interface IExpertiseService
    {
        Task<List<ExpertiseDTO>> GetAllExpertises();
        Task<ExpertiseDTO> GetById(int id);
        Task<ExpertiseDTO> CreateExpertise(ExpertiseDTO expertiseDTO);
        Task<ExpertiseDTO> UpdateExpertise(int id, ExpertiseDTO expertiseDTO);
        Task DeleteExpertise(int id);
    }
}
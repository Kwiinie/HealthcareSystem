using BusinessObjects.Entities;
using BusinessObjects.Enums;

namespace Repositories.Interfaces
{
    public interface IProfessionalDocumentRepository : IGenericRepository<ProfessionalDocument>
    {
        Task<List<ProfessionalDocument>> GetByProfessionalIdAsync(int professionalId);
        Task<List<ProfessionalDocument>> GetByVerificationStatusAsync(DocumentVerificationStatus status);
        Task<List<ProfessionalDocument>> GetExpiringDocumentsAsync(DateOnly cutoffDate);
    }
}

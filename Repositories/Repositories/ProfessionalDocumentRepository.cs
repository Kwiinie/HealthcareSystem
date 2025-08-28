using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects.DAOs;
using DataAccessObjects.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class ProfessionalDocumentRepository : GenericRepository<ProfessionalDocument>, IProfessionalDocumentRepository
    {
        private readonly IGenericDAO<ProfessionalDocument> _dao;

        public ProfessionalDocumentRepository(IGenericDAO<ProfessionalDocument> dao) : base(dao)
        {
            _dao = dao;
        }

        public async Task<List<ProfessionalDocument>> GetByProfessionalIdAsync(int professionalId)
        {
            var documents = await _dao.GetAllAsync();
            return documents.Where(d => d.ProfessionalId == professionalId)
                          .OrderByDescending(d => d.CreatedAt)
                          .ToList();
        }

        public async Task<List<ProfessionalDocument>> GetByVerificationStatusAsync(DocumentVerificationStatus status)
        {
            var documents = await _dao.GetAllAsync();
            return documents.Where(d => d.VerificationStatus == status)
                          .OrderByDescending(d => d.CreatedAt)
                          .ToList();
        }

        public async Task<List<ProfessionalDocument>> GetExpiringDocumentsAsync(DateOnly cutoffDate)
        {
            var documents = await _dao.GetAllAsync();
            return documents.Where(d => d.ExpiryDate.HasValue && d.ExpiryDate.Value <= cutoffDate)
                          .OrderBy(d => d.ExpiryDate)
                          .ToList();
        }
    }
}

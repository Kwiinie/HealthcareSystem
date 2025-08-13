using BusinessObjects.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        IGenericRepository<Article> ArticleRepository { get; }

        IGenericRepository<ArticleImage> ArticleImageRepository { get; } 
        IGenericRepository<Category> CategoryRepository { get; }

        IFacilityRepository FacilityRepository { get; }
        IProfessionalRepository ProfessionalRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IUserRepository UserRepository { get; }

        Task<int> SaveChangesAsync();
    } 
}

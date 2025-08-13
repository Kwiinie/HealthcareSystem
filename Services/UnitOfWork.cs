using DataAccessObjects.DAOs;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Repositories;
using BusinessObjects.Entities;
using System.Reflection.Metadata.Ecma335;

namespace Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FindingHealthcareSystemContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;

        public IGenericRepository<ArticleImage> ArticleImageRepository { get; set; }
        public IGenericRepository<Article> ArticleRepository { get; }
        public IGenericRepository<Category> CategoryRepository { get; }


        public UnitOfWork(FindingHealthcareSystemContext context, IFacilityRepository facilityRepository, 
                            IProfessionalRepository professionalRepository, IAppointmentRepository appointmentRepository, IUserRepository userRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();
            ArticleRepository = GetRepository<Article>();
            CategoryRepository = GetRepository<Category>();
            ArticleImageRepository = GetRepository<ArticleImage>();
            _facilityRepository = facilityRepository;
            _professionalRepository = professionalRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
        }


        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var dao = new GenericDAO<T>(_context);
                var repository = new GenericRepository<T>(dao);
                _repositories[typeof(T)] = repository;
            }

            return (IGenericRepository<T>)_repositories[typeof(T)];
        }

        public IFacilityRepository FacilityRepository => _facilityRepository;
        public IProfessionalRepository ProfessionalRepository => _professionalRepository;
        public IAppointmentRepository AppointmentRepository => _appointmentRepository;
        public IUserRepository UserRepository => _userRepository;


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;
using Services.Mappers;
using Services.Services;
using DataAccessObjects.Interfaces;
using DataAccessObjects.DAOs;
using Services;
using BusinessObjects.Entities;
using Services.Helpers;

namespace FindingHealthcareSystem
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
                
            ///////////////////////////////////////////////////////
            ///                      DAOS                      ///
            /////////////////////////////////////////////////////
            services.AddScoped<IFacilityDao, FacilityDao>();
            services.AddScoped<IProfessionalDao, ProfessionalDao>();
            services.AddScoped(typeof(IGenericDAO<>), typeof(GenericDAO<>));


            ////////////////////////////////////////////////////
            ///                 REPOSITORIES                ///
            //////////////////////////////////////////////////
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            

            ////////////////////////////////////////////////
            ///                 SERVICES                ///
            //////////////////////////////////////////////
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IFacilityTypeService, FacilityTypeService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddHttpContextAccessor();
            services.AddTransient<ILocationService, LocationService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IProfessionalService, ProfessionalService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IPublicServiceLayer, PublicServiceLayer>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddSingleton<VNPayHelper>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IExpertiseService, ExpertiseService>();
            services.AddScoped<IFileUploadService, FileUploadService>();



            return services;
        }
    }
}

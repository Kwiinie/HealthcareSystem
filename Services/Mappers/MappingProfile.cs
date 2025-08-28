using AutoMapper;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs;
using BusinessObjects.DTOs.Appointment;
using BusinessObjects.DTOs.Article;
using BusinessObjects.DTOs.Auth;
using BusinessObjects.DTOs.Auth;
using BusinessObjects.DTOs.Category;
using BusinessObjects.DTOs.Department;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Payment;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Schedule;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, GeneralUserDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<FacilityType, FacilityTypeDto>().ReverseMap();
            CreateMap<FacilityDepartment, FacilityDepartmentDto>().ReverseMap();
            CreateMap<Facility, FacilityDto>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();

            CreateMap<Article, ArticleDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Fullname))
            .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.ImgUrl))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.ImgUrls, opt => opt.MapFrom(src => src.ArticleImages.Select(ai => ai.ImgUrl).ToList()));
            CreateMap<ArticleDTO, Article>()
      .ForMember(dest => dest.Category, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.ArticleImages, opt => opt.Ignore());

            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Specialty, SpecialtyDto>().ReverseMap();
            CreateMap<PublicService, ServiceDto>().ReverseMap();
            CreateMap<PrivateService, ServiceDto>().ReverseMap();
            CreateMap<Patient, PatientDTO>().ReverseMap();
            CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src =>
                        src.Appointments != null && src.Appointments.Any()
                        ? src.Appointments.First().Id
                        : (int?)null
            ));
            CreateMap<Expertise, ExpertiseDTO>().ReverseMap();
            CreateMap<WorkingDate, WorkingDateDto>();
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<ScheduleException, ScheduleExceptionDto>();





            /////////////////////////////////////////////////////////////////////////
            ///MAPPING PROFESSIONAL EXPERTISE, SPECIALTY, USER INFO, SERVICE INFO///
            ///////////////////////////////////////////////////////////////////////
            CreateMap<Professional, ProfessionalDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Fullname))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
           .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.User.ImgUrl))
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.User.Status.ToString()))
           .ForMember(dest => dest.ExpertiseName, opt => opt.MapFrom(src => src.Expertise.Name))
           .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.ProfessionalSpecialties.Select(ps => ps.Specialty.Name).ToList()))
           .ForMember(dest => dest.PrivateServices, opt => opt.MapFrom(src => src.PrivateServices.Select(ps => new ServiceDto
           {
               Id = ps.Id,
               Name = ps.Name,
               Price = ps.Price,
               Description = ps.Description
           })
           .ToList()))
           .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules))
            .ForMember(dest => dest.ScheduleExceptions, opt => opt.MapFrom(src => src.ScheduleExceptions))
            .ForMember(dest => dest.ActiveSchedule, opt => opt.MapFrom<ActiveScheduleResolver>());


            ////////////////////////////////////////////////////////////////
            ///MAPPING FACILITY WITH TYPE, DEPARTMENT NAME, SERVICE INFO///
            //////////////////////////////////////////////////////////////
            CreateMap<Facility, SearchingFacilityDto>()
            .ForMember(dest => dest.FacilityTypeName, opt => opt.MapFrom(src => src.Type.Name))
            .ForMember(dest => dest.DepartmentNames, opt => opt.MapFrom(src => src.FacilityDepartments.Select(fd => fd.Department.Name).ToList()))
            .ForMember(dest => dest.PublicServices, opt => opt.MapFrom(src => src.PublicServices.Select(ps => new ServiceDto
            {
                Id = ps.Id,
                Name = ps.Name,
                Price = ps.Price,
                Description = ps.Description
            }).ToList()));

            // Mapping từ Facility -> FacilityDto
            CreateMap<Facility, FacilityDto>()
                .ForMember(dest => dest.FacilityDepartments,
                           opt => opt.MapFrom(src => src.FacilityDepartments));

            // Mapping từ FacilityDepartment -> FacilityDepartmentDto
            CreateMap<FacilityDepartment, FacilityDepartmentDto>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.Id))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department));



            /////////////////////////////////////////////////////////////////////////
            ///                     MAPPING APPOINTMENT PROFILE                  ///
            ///////////////////////////////////////////////////////////////////////
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();

            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();

            CreateMap<Appointment, MyAppointmentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date ?? DateTime.MinValue))
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.ProviderId))
                .ForMember(dest => dest.ProviderType, opt => opt.MapFrom(src => src.ProviderType))
                .ForMember(dest => dest.Service, opt => opt.MapFrom(src =>
                            src.ServiceType == ServiceType.Private
                            ? (object)src.PrivateService
                            : src.PublicService
                ))
                .ForMember(dest => dest.Professional, opt => opt.MapFrom(src => src.Professional))
                .ForMember(dest => dest.Facility, opt => opt.MapFrom(src => src.Facility))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.TransactionId : null))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Appointment, AppointmentDTO>()
    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
    .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date))
    .ForMember(d => d.ExpectedStart, opt => opt.MapFrom(s => s.ExpectedStart))
    .ForMember(d => d.CheckedInAt, opt => opt.MapFrom(s => s.CheckedInAt))
    .ForMember(d => d.StartAt, opt => opt.MapFrom(s => s.StartAt))
    .ForMember(d => d.EndAt, opt => opt.MapFrom(s => s.EndAt))
    .ForMember(d => d.TicketNo, opt => opt.MapFrom(s => s.TicketNo))
    .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
    .ForMember(d => d.Patient, opt => opt.MapFrom(s => s.Patient))
    .ForMember(d => d.Payment, opt => opt.MapFrom(s => s.Payment))
    .ForMember(d => d.PrivateService, opt => opt.MapFrom(s => s.PrivateService))
    .ForMember(d => d.PublicService, opt => opt.MapFrom(s => s.PublicService));

            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Patient, opt => opt.Ignore())
                .ForMember(d => d.Payment, opt => opt.Ignore())
                .ForMember(d => d.Professional, opt => opt.Ignore())
                .ForMember(d => d.Facility, opt => opt.Ignore())
                .ForMember(d => d.PrivateService, opt => opt.Ignore())
                .ForMember(d => d.PublicService, opt => opt.Ignore())
                .ForMember(d => d.ExpectedStart, opt => opt.MapFrom(s => s.ExpectedStart != default ? s.ExpectedStart : (s.Date ?? default)))
                .ForMember(d => d.Date, opt => opt.MapFrom(s => s.Date));


            CreateMap<Appointment, RescheduleAppointmentDTO>().ReverseMap();

        }
    }
}

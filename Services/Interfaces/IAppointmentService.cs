using BusinessObjects.Commons;
using BusinessObjects.DTOs;
using BusinessObjects.DTOs.Appointment;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Interfaces
{
    public interface IAppointmentService
    {
        // CREATE
        Task<Result<AppointmentDTO>> AddAsync(CreateAppointmentDto entity);
        Task<bool> AddAsync(RescheduleAppointmentDTO reschedule);

        // READ
        Task<Result<AppointmentDTO>> GetByIdAsync(int id);
        Task<List<AppointmentDTO>> GetAllAsync();
        Task<List<AppointmentDTO>> GetAllAppoinmentByDate(int id, DateTime startDate, DateTime endDate);
        Task<List<AppointmentDTO>> GetAppointmentsByProviderAndDate(int providerId, string providerType, DateTime date);
        Task<List<string>> GetSlotsExistedByDate(DateTime date, List<string> slots);
        Task<List<MyAppointmentDto>> GetMyAppointment(int userId);
        Task<MyAppointmentDto?> GetMySpecificAppointment(int appointmentId);
        Task<AppointmentDTO> GetAppointmentByDateAndSlot(int id, ServiceType type);
        Task<(int, List<AppointmentDTO>)> GetPagenagingAppointments(int id, int pagee, int size);

        // UPDATE
        Task UpdateAppointmentDiagnose(int id, string diagnose);
        Task<bool> ChangeAppointmentStatus(int id, AppointmentStatus newStatus); // giữ method cũ
        Task<Result<bool>> UpdateStatusAsync(int id, AppointmentStatus newStatus); // method mới có Result
        Task<Result<bool>> CheckInAsync(int id);
        Task<Result<bool>> CancelAsync(int id);

        // COUNT/AGG
        Task<int> CountAppointmentByStatus(int id, string status);
        Task<int> CountTotalPatient(int id);
    }
}

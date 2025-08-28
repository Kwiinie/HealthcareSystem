using AutoMapper;
using BusinessObjects.Commons;
using BusinessObjects.DTOs.Appointment;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IAppointmentRepository appointmentRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
        }

        /// <summary>
        /// CREATE APPOINTMENT
        /// </summary>
        public async Task<Result<AppointmentDTO>> AddAsync(CreateAppointmentDto entity)
        {
            try
            {
                // 1) Map PatientId từ userId -> patient.Id
                var patientRepo = _unitOfWork.GetRepository<Patient>();
                var patient = await patientRepo.FindAsync(p => p.UserId == entity.PatientId);
                if (patient == null)
                    return Result<AppointmentDTO>.ErrorResult("Invalid Patient ID.");

                // 2) Validate provider & set ServiceType
                if (entity.ProviderType == ProviderType.Professional)
                {
                    var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(entity.ProviderId!.Value);
                    if (professional == null)
                        return Result<AppointmentDTO>.ErrorResult("Invalid Professional ID.");

                    entity.ProviderType = ProviderType.Professional;
                    entity.ServiceType = ServiceType.Private;
                }
                else if (entity.ProviderType == ProviderType.Facility)
                {
                    var facility = await _unitOfWork.FacilityRepository.GetByIdWithRelationsAsync(entity.ProviderId!.Value);
                    if (facility == null)
                        return Result<AppointmentDTO>.ErrorResult("Invalid Facility ID.");

                    entity.ProviderType = ProviderType.Facility;
                    entity.ServiceType = ServiceType.Public;
                }
                else
                {
                    return Result<AppointmentDTO>.ErrorResult("Invalid ProviderType.");
                }

                // 3) Status + PatientId (thực thể patient.Id)
                entity.Status = AppointmentStatus.Scheduled;
                entity.PatientId = patient.Id;

                // 4) Map sang entity
                var appointmentEntity = _mapper.Map<Appointment>(entity);
                appointmentEntity.Patient = patient;

                // 5) Đảm bảo ExpectedStart có giá trị (khớp entity)
                if (appointmentEntity.ExpectedStart == default)
                {
                    // Ưu tiên Date nếu CreateAppointmentDto gửi Date (SelectedDate+TimeSlot)
                    appointmentEntity.ExpectedStart = entity.Date ?? DateTime.UtcNow;
                }

                // 6) Nguồn đặt lịch
                if (appointmentEntity.Source == default)
                    appointmentEntity.Source = AppointmentSource.Booked;

                // 7) Ghi DB
                await _unitOfWork.AppointmentRepository.AddAsync(appointmentEntity);
                await _unitOfWork.SaveChangesAsync();

                var appointmentDTO = _mapper.Map<AppointmentDTO>(appointmentEntity);
                return Result<AppointmentDTO>.SuccessResult(appointmentDTO);
            }
            catch (Exception ex)
            {
                return Result<AppointmentDTO>.ErrorResult($"An error occurred while creating the appointment: {ex.Message}");
            }
        }

        public async Task<bool> AddAsync(RescheduleAppointmentDTO reschedule)
        {
            try
            {
                var entity = _mapper.Map<Appointment>(reschedule);

                // giữ chặt ExpectedStart nếu DTO có Date
                if (entity.ExpectedStart == default && entity.Date.HasValue)
                    entity.ExpectedStart = entity.Date.Value;

                await _unitOfWork.AppointmentRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ========= READ =========

        public async Task<Result<AppointmentDTO>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.AppointmentRepository
                    .FindAsync(a => a.Id == id,
                        "Facility,Professional,PrivateService,PublicService,Patient,Patient.User,Payment");

                if (entity == null)
                    return Result<AppointmentDTO>.ErrorResult("Appointment not found.");

                var dto = _mapper.Map<AppointmentDTO>(entity);
                return Result<AppointmentDTO>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return Result<AppointmentDTO>.ErrorResult($"Error loading appointment: {ex.Message}");
            }
        }

        public async Task<List<AppointmentDTO>> GetAllAsync()
        {
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAllAppoinmentByDate(int id, DateTime startDate, DateTime endDate)
        {
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAppoinmentByDate(id, startDate, endDate);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByProviderAndDate(int providerId, string providerType, DateTime date)
        {
            ProviderType type = (ProviderType)Enum.Parse(typeof(ProviderType), providerType);
            var appointments = await _appointmentRepository.GetAppointmentsByProviderAndDateAsync(type, providerId, date);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<string>> GetSlotsExistedByDate(DateTime date, List<string> slots)
        {
            var slotsInDay = await _appointmentRepository
                .Query()
                .Where(x => x.Date.HasValue &&
                            x.Date.Value.Date == date.Date)
                .ToListAsync();

            var slotsExisted = slotsInDay
                .Select(x => x.Date!.Value.ToString("HH:mm"))
                .Where(slot => slots.Contains(slot))
                .ToList();

            return slotsExisted;
        }

        public async Task<List<MyAppointmentDto>> GetMyAppointment(int userId)
        {
            var patientRepo = _unitOfWork.GetRepository<Patient>();
            var patient = await patientRepo.FindAsync(p => p.UserId == userId);
            var appointments = await _unitOfWork.AppointmentRepository.GetMyAppointment(patient.Id);
            return _mapper.Map<List<MyAppointmentDto>>(appointments);
        }

        public async Task<MyAppointmentDto?> GetMySpecificAppointment(int appointmentId)
        {
            var appointment = await _unitOfWork.AppointmentRepository
                .FindAsync(a => a.Id == appointmentId, "Facility,Professional,PrivateService,PublicService,Payment");
            return _mapper.Map<MyAppointmentDto>(appointment);
        }

        public async Task<AppointmentDTO> GetAppointmentByDateAndSlot(int id, ServiceType type)
        {
            var query = _unitOfWork.AppointmentRepository.Query()
                        .Where(x => x.Id == id)
                        .Include(x => x.Patient).ThenInclude(x => x.User)
                        .Include(x => x.Payment)
                        .AsQueryable();

            if (type == ServiceType.Private)
            {
                query = query.Include(x => x.PrivateService);
            }
            else
            {
                query = query.Include(x => x.PublicService);
            }

            var appointment = await query.FirstOrDefaultAsync();
            return _mapper.Map<AppointmentDTO>(appointment);
        }

        public async Task<(int, List<AppointmentDTO>)> GetPagenagingAppointments(int id, int pagee, int size)
        {
            var list = await _unitOfWork.AppointmentRepository.Query()
                .Include(x => x.Professional)
                .Where(x => x.Professional.UserId == id && x.ProviderId == x.Professional.Id && x.Status == AppointmentStatus.Completed)
                .Include(x => x.Patient)
                .Include(x => x.Patient.User)
                .ToListAsync();

            var maxPage = (int)Math.Floor((decimal)list.Count / size);
            var pageItems = list.OrderByDescending(x => x.Date)
                                .Skip((pagee - 1) * size)
                                .Take(size)
                                .ToList();

            return (maxPage, _mapper.Map<List<AppointmentDTO>>(pageItems));
        }

        // ========= UPDATE =========

        public async Task UpdateAppointmentDiagnose(int id, string diagnose)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null) return;

            appointment.Description = diagnose;
            _unitOfWork.AppointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ChangeAppointmentStatus(int id, AppointmentStatus newStatus)
        {
            try
            {
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
                if (appointment == null) return false;

                appointment.Status = newStatus;
                _unitOfWork.AppointmentRepository.Update(appointment);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Result<bool>> UpdateStatusAsync(int id, AppointmentStatus newStatus)
        {
            try
            {
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
                if (appointment == null)
                    return Result<bool>.ErrorResult("Appointment not found.");

                appointment.Status = newStatus;

                // Gán mốc thời gian & phát số thứ tự khi Check-in (nếu bạn muốn)
                if (newStatus == AppointmentStatus.CheckedIn)
                {
                    if (appointment.CheckedInAt == null)
                        appointment.CheckedInAt = DateTime.UtcNow;

                    // Phát TicketNo nếu chưa có
                    if (appointment.TicketNo == null)
                    {
                        var apptDate = (appointment.Date ?? appointment.ExpectedStart).Date;

                        var sameDayQuery = _unitOfWork.AppointmentRepository.Query()
                            .Where(a =>
                                a.ProviderId == appointment.ProviderId &&
                                a.ProviderType == appointment.ProviderType &&
                                (a.Date ?? a.ExpectedStart).Date == apptDate);

                        // Đếm những lịch đã có ticket hoặc đã check-in/đang khám/hoàn tất
                        var count = await sameDayQuery
                            .Where(a => a.TicketNo != null
                                     || a.Status == AppointmentStatus.CheckedIn
                                     || a.Status == AppointmentStatus.InExam
                                     || a.Status == AppointmentStatus.Completed)
                            .CountAsync();

                        appointment.TicketNo = count + 1;
                    }
                }

                // Timestamp khi vào khám / kết thúc khám (tuỳ logic)
                if (newStatus == AppointmentStatus.InExam && appointment.StartAt == null)
                    appointment.StartAt = DateTime.UtcNow;

                if (newStatus == AppointmentStatus.Completed && appointment.EndAt == null)
                    appointment.EndAt = DateTime.UtcNow;

                _unitOfWork.AppointmentRepository.Update(appointment);
                await _unitOfWork.SaveChangesAsync();
                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult($"Failed to update status: {ex.Message}");
            }
        }

        public Task<Result<bool>> CheckInAsync(int id)
            => UpdateStatusAsync(id, AppointmentStatus.CheckedIn);

        public Task<Result<bool>> CancelAsync(int id)
            => UpdateStatusAsync(id, AppointmentStatus.CancelledByPatient);

        // ========= COUNT =========

        public async Task<int> CountAppointmentByStatus(int id, string status)
        {
            return await _unitOfWork.AppointmentRepository.CountAppointmentByStatus(id, status);
        }

        public async Task<int> CountTotalPatient(int id)
        {
            return await _unitOfWork.AppointmentRepository.Query()
                .Include(x => x.Professional)
                .Where(x => x.Professional.UserId == id && x.ProviderId == x.Professional.Id)
                .Select(x => x.PatientId)
                .Distinct()
                .CountAsync();
        }
    }
}

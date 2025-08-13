using BusinessObjects.Commons;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Professional : BaseEntity
{
    public int? UserId { get; set; }

    public int? ExpertiseId { get; set; }

    public string? Province { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? Address { get; set; }

    public string? Degree { get; set; }

    public string? Experience { get; set; }

    public string? WorkingHours { get; set; } /// giờ làm vieecjj khoảng thời gian 

    public ProfessionalRequestStatus RequestStatus { get; set; }

    public virtual Expertise? Expertise { get; set; }

    public virtual ICollection<PrivateService> PrivateServices { get; set; } = new List<PrivateService>();

    public virtual ICollection<ProfessionalSpecialty> ProfessionalSpecialties { get; set; } = new List<ProfessionalSpecialty>();

    public virtual User? User { get; set; }

    // Thêm các thuộc tính từ User
    public string? Fullname => User?.Fullname;
    public string? Email => User?.Email;
    public string? PhoneNumber => User?.PhoneNumber;
    public string? Gender => User?.Gender;
    public DateOnly? Birthday => User?.Birthday;
}


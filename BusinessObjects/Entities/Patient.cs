using BusinessObjects.Commons;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Patient : BaseEntity
{
    public int? UserId { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User? User { get; set; }

    // Thêm các thuộc tính từ User
    public string? Fullname => User?.Fullname;
    public string? Email => User?.Email;
    public string? PhoneNumber => User?.PhoneNumber;
    public string? Gender => User?.Gender;
    public DateOnly? Birthday => User?.Birthday;

    public static implicit operator Patient(User v)
    {
        throw new NotImplementedException();
    }
}

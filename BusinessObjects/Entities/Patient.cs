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

    public string? Fullname {  get; set; }
    public string? Gender {  get; set; }
    public DateOnly? Birthday {  get; set; }

}

using BusinessObjects.Commons;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class User : BaseEntity
{
    public Role Role { get; set; }

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public UserStatus Status { get; set; }

    public string? ImgUrl { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual Patient? Patient { get; set; }

    public virtual Professional? Professional { get; set; }
}

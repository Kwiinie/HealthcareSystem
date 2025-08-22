using BusinessObjects.Commons;

namespace BusinessObjects.Entities;

public class EmailVerificationToken : BaseEntity
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; } = false;
    
    public virtual User User { get; set; } = null!;
}

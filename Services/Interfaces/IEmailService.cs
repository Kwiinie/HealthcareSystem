using BusinessObjects.Commons;

namespace Services.Interfaces
{
    public interface IEmailService
    {
        Task<Result<bool>> SendEmailVerificationAsync(string email, string fullName, string verificationToken);
        Task<Result<bool>> SendPasswordResetEmailAsync(string email, string fullName, string resetToken);
        Task<Result<bool>> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
    }
}

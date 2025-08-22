using BusinessObjects.Commons;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using Services.Setups;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<Result<bool>> SendEmailVerificationAsync(string email, string fullName, string verificationToken)
        {
            var verificationUrl = $"{_emailSettings.BaseUrl}/Auth/VerifyEmail?token={verificationToken}&email={Uri.EscapeDataString(email)}";
            
            var subject = "Xác nhận địa chỉ email của bạn - Healthcare System";
            var body = GenerateEmailVerificationTemplate(fullName, verificationUrl);

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<Result<bool>> SendPasswordResetEmailAsync(string email, string fullName, string resetToken)
        {
            var resetUrl = $"{_emailSettings.BaseUrl}/Auth/ResetPassword?token={resetToken}&email={Uri.EscapeDataString(email)}";
            
            var subject = "Đặt lại mật khẩu - Healthcare System";
            var body = GeneratePasswordResetTemplate(fullName, resetUrl);

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<Result<bool>> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                client.EnableSsl = _emailSettings.EnableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult($"Không thể gửi email: {ex.Message}");
            }
        }

        private string GenerateEmailVerificationTemplate(string fullName, string verificationUrl)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Xác nhận email</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; background: #f5fdf7; }}
        .container {{ background: #ffffff; padding: 30px; border-radius: 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.05); }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .logo {{ color: #2e7d32; font-size: 24px; font-weight: bold; }}
        .content {{ background: #fdfefc; padding: 30px; border-radius: 8px; margin: 20px 0; }}
        .button {{ display: inline-block; padding: 12px 30px; background: #4caf50; color: white; text-decoration: none; border-radius: 6px; margin: 20px 0; font-weight: bold; }}
        .button:hover {{ background: #388e3c; }}
        .footer {{ text-align: center; margin-top: 30px; font-size: 14px; color: #666; }}
        .warning {{ background: #e8f5e9; border: 1px solid #c8e6c9; padding: 15px; border-radius: 5px; margin: 20px 0; color: #2e7d32; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>Healthcare System</div>
        </div>
        
        <div class='content'>
            <h2>Chào {fullName}!</h2>
            <p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>Healthcare System</strong>. Để hoàn tất quá trình đăng ký, vui lòng xác nhận địa chỉ email của bạn.</p>
            
            <div style='text-align: center;'>
                <a href='{verificationUrl}' class='button'>Xác nhận địa chỉ email</a>
            </div>
            
            <div class='warning'>
                <strong>Lưu ý:</strong> Link xác nhận này sẽ hết hạn sau 24 giờ. Nếu bạn không xác nhận trong thời gian này, bạn sẽ cần đăng ký lại.
            </div>
            
            <p>Nếu bạn không thể nhấp vào nút trên, hãy sao chép và dán liên kết sau vào trình duyệt:</p>
            <p style='word-break: break-all; color: #555; font-size: 14px;'>{verificationUrl}</p>
            
            <p>Nếu bạn không yêu cầu tạo tài khoản này, vui lòng bỏ qua email này.</p>
        </div>
        
        <div class='footer'>
            <p>&copy; 2025 Healthcare System. Tất cả quyền được bảo lưu.</p>
            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
        </div>
    </div>
</body>
</html>";
        }


        private string GeneratePasswordResetTemplate(string fullName, string resetUrl)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Đặt lại mật khẩu</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; background: #f5fdf7; }}
        .container {{ background: #ffffff; padding: 30px; border-radius: 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.05); }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .logo {{ color: #2e7d32; font-size: 24px; font-weight: bold; }}
        .content {{ background: #fdfefc; padding: 30px; border-radius: 8px; margin: 20px 0; }}
        .button {{ display: inline-block; padding: 12px 30px; background: #43a047; color: white; text-decoration: none; border-radius: 6px; margin: 20px 0; font-weight: bold; }}
        .button:hover {{ background: #2e7d32; }}
        .footer {{ text-align: center; margin-top: 30px; font-size: 14px; color: #666; }}
        .warning {{ background: #fff3cd; border: 1px solid #ffeeba; padding: 15px; border-radius: 5px; margin: 20px 0; color: #856404; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>Healthcare System</div>
        </div>
        
        <div class='content'>
            <h2>Chào {fullName}!</h2>
            <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
            
            <div style='text-align: center;'>
                <a href='{resetUrl}' class='button'>Đặt lại mật khẩu</a>
            </div>
            
            <div class='warning'>
                <strong>Bảo mật:</strong> Link này sẽ hết hạn sau 1 giờ. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này và mật khẩu của bạn sẽ không thay đổi.
            </div>
            
            <p>Nếu bạn không thể nhấp vào nút trên, hãy sao chép và dán liên kết sau vào trình duyệt:</p>
            <p style='word-break: break-all; color: #555; font-size: 14px;'>{resetUrl}</p>
        </div>
        
        <div class='footer'>
            <p>&copy; 2025 Healthcare System. Tất cả quyền được bảo lưu.</p>
            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
        </div>
    </div>
</body>
</html>";
        }

    }
}

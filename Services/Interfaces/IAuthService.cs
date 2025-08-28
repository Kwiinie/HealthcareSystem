using BusinessObjects.Commons;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<GeneralUserDto>> LoginAsync(LoginDto loginDto);
        Task<Result<bool>> SendEmailVerificationAsync(int userId, string email, string fullName);
        Task<Result<bool>> VerifyEmailAsync(string token, string email);
        Task<Result<bool>> ResendEmailVerificationAsync(string email);
    }
}

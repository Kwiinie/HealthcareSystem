using AutoMapper;
using BusinessObjects.Commons;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Auth;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        public async Task<Result<GeneralUserDto>> LoginAsync(LoginDto loginDto)
        {
            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.FindAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Result<GeneralUserDto>.ErrorResult("Tài khoản không tồn tại");
            }

            else if (user.Password == loginDto.Password) 
            {
                if (user.Status == UserStatus.Inactive)
                {
                    return Result<GeneralUserDto>.ErrorResult("Tài khoản đã bị khóa");
                }

                if (!user.IsVerified)
                {
                    return Result<GeneralUserDto>.ErrorResult("Email chưa được xác nhận. Vui lòng kiểm tra email để xác nhận tài khoản.");
                }

                var userDto = _mapper.Map<GeneralUserDto>(user);

                return Result<GeneralUserDto>.SuccessResult(userDto);
            }
            else
            {
                return Result<GeneralUserDto>.ErrorResult("Sai email hoặc mật khẩu");
            }
        }

        public async Task<Result<bool>> SendEmailVerificationAsync(int userId, string email, string fullName)
        {
            try
            {
                var token = GenerateSecureToken();
                var expiryDate = DateTime.UtcNow.AddHours(24); // 24h

                var tokenRepo = _unitOfWork.GetRepository<EmailVerificationToken>();
                var verificationToken = new EmailVerificationToken
                {
                    UserId = userId,
                    Email = email,
                    Token = token,
                    ExpiryDate = expiryDate,
                    IsUsed = false
                };

                await tokenRepo.AddAsync(verificationToken);
                await _unitOfWork.SaveChangesAsync();

                //gui mail
                var emailResult = await _emailService.SendEmailVerificationAsync(email, fullName, token);
                if (!emailResult.IsSuccess)
                {
                    return Result<bool>.ErrorResult($"Không thể gửi email xác nhận: {emailResult.ErrorMessage}");
                }

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult($"Lỗi khi gửi email xác nhận: {ex.Message}");
            }
        }

        public async Task<Result<bool>> VerifyEmailAsync(string token, string email)
        {
            try
            {
                var tokenRepo = _unitOfWork.GetRepository<EmailVerificationToken>();
                var userRepo = _unitOfWork.GetRepository<User>();

                var verificationToken = await tokenRepo.FindAsync(t => 
                    t.Token == token && 
                    t.Email == email && 
                    !t.IsUsed);

                if (verificationToken == null)
                {
                    return Result<bool>.ErrorResult("Token xác nhận không hợp lệ hoặc đã được sử dụng.");
                }

                if (verificationToken.ExpiryDate < DateTime.UtcNow)
                {
                    return Result<bool>.ErrorResult("Token xác nhận đã hết hạn. Vui lòng yêu cầu gửi lại email xác nhận.");
                }

                var user = await userRepo.FindAsync(u => u.Id == verificationToken.UserId);
                if (user == null)
                {
                    return Result<bool>.ErrorResult("Người dùng không tồn tại.");
                }

                user.IsVerified = true;
                userRepo.Update(user);

                verificationToken.IsUsed = true;
                tokenRepo.Update(verificationToken);

                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult($"Lỗi khi xác nhận email: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ResendEmailVerificationAsync(string email)
        {
            try
            {
                var userRepo = _unitOfWork.GetRepository<User>();
                var user = await userRepo.FindAsync(u => u.Email == email);

                if (user == null)
                {
                    return Result<bool>.ErrorResult("Email không tồn tại trong hệ thống.");
                }

                if (user.IsVerified)
                {
                    return Result<bool>.ErrorResult("Email đã được xác nhận.");
                }

                var tokenRepo = _unitOfWork.GetRepository<EmailVerificationToken>();
                var oldTokens = await tokenRepo.FindAllAsync(t => t.Email == email && !t.IsUsed);
                foreach (var oldToken in oldTokens)
                {
                    oldToken.IsUsed = true;
                    tokenRepo.Update(oldToken);
                }
                await _unitOfWork.SaveChangesAsync();

                var result = await SendEmailVerificationAsync(user.Id, user.Email!, user.Fullname!);
                return result;
            }
            catch (Exception ex)
            {
                return Result<bool>.ErrorResult($"Lỗi khi gửi lại email xác nhận: {ex.Message}");
            }
        }

        private string GenerateSecureToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var tokenBytes = new byte[32];
            rng.GetBytes(tokenBytes);
            return Convert.ToBase64String(tokenBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}

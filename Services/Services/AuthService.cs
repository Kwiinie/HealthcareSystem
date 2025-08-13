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
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

                var userDto = _mapper.Map<GeneralUserDto>(user);

                return Result<GeneralUserDto>.SuccessResult(userDto);
            }
            else
            {
                return Result<GeneralUserDto>.ErrorResult("Sai email hoặc mật khẩu");
            }
        }
    }
}

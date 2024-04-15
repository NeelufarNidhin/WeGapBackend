using System;
using Microsoft.AspNetCore.Identity;
using WeGapApi.Models.Dto;

namespace WeGapApi.Services.Services.Interface
{
	public interface IAuthenticationService
	{
		Task<IdentityResult> Register(SignUpRequestDto signupRequestDto);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
		Task<bool> ResendOTP(string userId);
		Task<bool> LoginWithOTP(string otp);
	}
}


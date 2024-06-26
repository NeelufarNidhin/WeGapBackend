using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WeGapApi.Data;
using WeGapApi.Models;
using WeGapApi.Models.Dto;
using WeGapApi.Services.Services.Interface;
using WeGapApi.Utility;

namespace WeGapApi.Services
{
	public class AuthenticationService : IAuthenticationService
	{
        private readonly ApplicationDbContext _db;
       
        private string secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly ClientConfiguration _clientConfiguration;

        public AuthenticationService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
             IConfiguration configuration, IEmailSender emailSender, IOptions<ClientConfiguration> clientConfiguration)
		{
            _db = db;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _emailSender = emailSender;
            _clientConfiguration = clientConfiguration.Value;
           // _httpContextAccessor = httpContextAccessor;
          
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDto.Password);

            if (!isValid)
                throw new InvalidOperationException("UserName or Password is incorrect");

            if (userFromDb.IsBlocked)
            {
                throw new InvalidOperationException("User Blocked, Please contact Adminstrator");
            }

            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", userFromDb.Id.ToString()),
                    new Claim("firstName", userFromDb.FirstName.ToString()),
                    new Claim("lastName", userFromDb.LastName.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim("isBlocked", userFromDb.IsBlocked.ToString())
                }),

               Expires = DateTime.UtcNow.AddDays(7),
               // Expires = DateTime.UtcNow.AddSeconds(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


            LoginResponseDto loginResponse = new()
            {
                Email = userFromDb.Email,
                Role = userFromDb.Role,
                Token = tokenHandler.WriteToken(token)
            };


            if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                throw new InvalidOperationException("Please check the user credentials");
            }

            return loginResponse;

        }

        public async Task<bool> LoginWithOTP(string otp)
        {
            
            var user =  _db.ApplicationUsers.FirstOrDefault(u => u.OTP == otp);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid Otp");
            }

            user.IsProfile = true;
            user.OTP = null;
            _db.SaveChanges();

            return true;
        }

        public async Task<IdentityResult> Register(SignUpRequestDto signupRequestDto)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == signupRequestDto.UserName.ToLower());
            if (userFromDb != null)
            {
                throw new InvalidOperationException("User Name already exists");
            }

            ApplicationUser user = new()
            {
                FirstName = signupRequestDto.FirstName,
                LastName = signupRequestDto.LastName,
                UserName = signupRequestDto.UserName,
                Email = signupRequestDto.UserName,
                NormalizedEmail = signupRequestDto.UserName.ToUpper(),
                Role = signupRequestDto.Role,
                TwoFactorEnabled = true,
                IsProfile  = false
            };

            var otptoken = GenerateRandomOtp(); 

            user.OTP = otptoken;

            var result = await _userManager.CreateAsync(user, signupRequestDto.Password);

            if (!result.Succeeded)
            {
               
                throw new InvalidOperationException("Failed to register user");
            }



            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        


                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employer));
                    }


                    if (signupRequestDto.Role.ToLower() == SD.Role_Admin)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Admin);

                    }
                    if (signupRequestDto.Role.ToLower() == SD.Role_Employer)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Employer);

                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Employee);

                    }
                  
                    await _emailSender.SendEmailAsync(user.Email, "OTP Confirmation", otptoken);
                    return result;


        }


        public async Task<bool> ResendOTP(string email)
        {
            

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == email);

            if (user is null)
            {
                throw new InvalidOperationException("User not found");
            }

            var otptoken = GenerateRandomOtp();
            user.OTP = otptoken;
            _db.SaveChanges();

            await _emailSender.SendEmailAsync(user.Email, "New OTP", $"Your new OTP is: {otptoken}");
           

            return true;
        }


        private string GenerateRandomOtp(int length = 6)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new InvalidOperationException($"User with email: {email} does not exists");

            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = $"{_clientConfiguration.Url}/resetpassword?code={code}&userid={user.Id}";
           

            await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"<h1>Reset Password</h1><p>Click <a href =\"{link}\">here</a> to reset password.</p>");

        
        }
        
        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByIdAsync(resetPassword.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with Id: {resetPassword.UserId} does not exists");

            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPassword.Code));

            var result = await _userManager.ResetPasswordAsync(user, code, resetPassword.NewPassword);

            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(user.Email, "Password Reset Complete", $"<p>Your password reset successfully.</p>");
            }

            return result;

        }
    }
}


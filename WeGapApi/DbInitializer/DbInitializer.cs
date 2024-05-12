using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeGapApi.Data;
using WeGapApi.Models;
using WeGapApi.Utility;

namespace WeGapApi.DbInitializer
{
	public class DbInitializer : IDbInitializer
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
		public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IConfiguration configuration)
		{
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;

		}

        public void Initialize()
        {
            //apply pending migration

            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
            }


            //create roles if roles are not there
            if (!_roleManager.RoleExistsAsync(SD.Role_Employee).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employer)).GetAwaiter().GetResult();



                //create admin user
                   var adminPwd = _configuration.GetValue<string>("AdminPwd");
               // var adminPwd = Environment.GetEnvironmentVariable("ADMINPWD");

                _userManager.CreateAsync(new ApplicationUser
                {
                    FirstName = "Neelufar",
                    LastName = "Nidhin",
                    Role = SD.Role_Admin,
                    Email = "nilu.nidhin@gmail.com",
                    UserName = "nilu.nidhin@gmail.com",
                    NormalizedEmail = "NILU.NIDHIN@GMAIL.COM",
                    CreateAt = DateTime.UtcNow,
                    TwoFactorEnabled = true,
                    IsBlocked = false
                }, adminPwd).GetAwaiter().GetResult();

                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "nilu.nidhin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}


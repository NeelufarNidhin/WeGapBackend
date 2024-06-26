using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WeGapApi.Models
{
	public class ApplicationUser : IdentityUser
	{
        [Required(ErrorMessage = "First Name is a required !!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name must contain only characters")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name  must contain only characters")]
        [Required(ErrorMessage = "Last name is a required !!")]
        public string LastName { get; set; }
        public string Role { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
		public string Createby { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsProfile { get; set; }
        public string OTP { get; set; }
    }
}


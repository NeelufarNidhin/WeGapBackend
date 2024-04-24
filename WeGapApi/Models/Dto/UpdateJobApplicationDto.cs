using System;
using System.ComponentModel.DataAnnotations;

namespace WeGapApi.Models.Dto
{
	public class UpdateJobApplicationDto
	{
       
        [Required]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        public Guid JobId { get; set; }
        public Job Job { get; set; }
       
        public string JobStatus { get; set; }
        public string Jobtitle { get; set; }
        public string Employer { get; set; }
    }
}


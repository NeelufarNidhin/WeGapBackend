using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeGapApi.Models.Dto
{
	public class AddJobApplicationDto
	{

       
        public Guid EmployeeId { get; set; }
       
        public Guid JobId { get; set; }
        public Guid Employer { get; set; }

        [Required]
        public string Jobtitle { get; set; }
        [Required]
        public string Availability { get; set; }
        [MaxLength(1000)]
        public string CoverLetter { get; set; }
        
        public IFormFile Resume { get; set; }
        public string ResumeFileName { get; set; }
        public string JobStatus { get; set; }
    }
}


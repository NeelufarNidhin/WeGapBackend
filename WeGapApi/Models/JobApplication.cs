using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeGapApi.Models
{
    public class JobApplication
    {

        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
       
        public Guid Employer { get; set; }
        public Guid JobId { get; set; }
        public Job Job { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
        
        public string Jobtitle { get; set; }
       
        public string Availability { get; set; }
        [MaxLength(1000)]
        public string CoverLetter {get;set;}
        [NotMapped]
        public IFormFile Resume { get; set; }
        public string ResumeFileName { get; set; }
        public string JobStatus { get; set; }
    }
}


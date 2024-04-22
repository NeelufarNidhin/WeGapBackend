using System;
using System.ComponentModel.DataAnnotations;

namespace WeGapApi.Models.Dto
{
	public class AddJobDto
	{
        [Required(ErrorMessage = "JobTitle is required")]
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public string Salary { get; set; }


        public Guid EmployerId { get; set; }
        public List<Guid> JobSkill { get; set; }


        public Guid JobTypeId { get; set; }


        public List<JobJobSkill> JobJobSkill { get; set; }
       
      
    }
}


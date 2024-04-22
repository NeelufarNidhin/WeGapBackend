using System;
using System.ComponentModel.DataAnnotations;

namespace WeGapApi.Models.Dto
{
	public class JobDto
	{
        public Guid Id { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public string Salary { get; set; }
        public DateTime CreatedAt { get; set; }

        //Navigation property
        public Guid EmployerId { get; set; }
        public Employer Employer { get; set; }

        public Guid JobTypeId { get; set; }
        //
        public JobType JobType { get; set; }

        public List<JobJobSkill> JobJobSkill { get; set; }

    }
}


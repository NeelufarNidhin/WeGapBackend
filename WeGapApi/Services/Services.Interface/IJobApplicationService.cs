using System;
using WeGapApi.Models;
using WeGapApi.Models.Dto;

namespace WeGapApi.Services.Services.Interface
{
	public interface IJobApplicationService
	{
        Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationAsync();
        Task<JobApplicationDto> CreateJobApplicationAsync(AddJobApplicationDto addJobApplicationDto);
        Task<JobApplicationDto> GetJobApplicationById(Guid id);
        Task<JobApplicationDto> UpdateJobApplication(Guid id, UpdateJobApplicationDto updateJobApplicationDto);
        Task<JobApplicationDto> DeleteJobApplication(Guid id);
    }
}


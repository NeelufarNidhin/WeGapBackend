using System;
using WeGapApi.Models;
using WeGapApi.Models.Dto;

namespace WeGapApi.Services.Services.Interface
{
	public interface IJobService
	{
        Task<List<JobDto>> GetAllJobsAsync(int pageSize,int pageNumber);
        Task<JobDto> GetJobsByIdAsync(Guid id);
        Task<JobDto> AddJobsAsync(AddJobDto job);
        Task<JobDto> UpdateJobsAsync(Guid id, UpdateJobDto job);
        Task<JobDto> DeleteJobsAsync(Guid id);
        Task<List<JobSkillDto>> GetJobJobSkillsByIdAsync(Guid jobId);
        Task<List<JobDto>>  GetJobsByEmployerId(Guid id);
        Task<List<JobDto>> GetTotalJobs();
        Task<List<JobDto>> GetSearchQuery(string searchString);
        Task<List<JobDto>> GetJobType(string jobType);
        Task<List<JobDto>> GetJobSkill(string jobSkill);
    }
}


using System;
using WeGapApi.Models;
using WeGapApi.Models.Dto;

namespace WeGapApi.Repository.Interface
{
	public interface IJobRepository
	{
        Task<List<Job>> GetAllJobsAsync();
        Task<Job> GetJobsByIdAsync(Guid id);
        Task<Job> AddJobsAsync(Job job);
        Task<Job> UpdateJobsAsync(Guid id, Job job);
        Task<Job> DeleteJobsAsync(Guid id);
        Task<List<Job>> GetJobsByEmployerId(Guid id);
         Task <List<Job>> GetSearchQuery(string searchString);
    }
}


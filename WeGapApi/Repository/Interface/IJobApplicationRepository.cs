using System;
using WeGapApi.Models;

namespace WeGapApi.Repository.Interface
{
	public interface IJobApplicationRepository
	{
		Task<IEnumerable<JobApplication>> GetAllJobApplicationAsync();
		Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication);
		Task<JobApplication> GetJobApplicationById(Guid id);
		Task<JobApplication> UpdateJobApplication(Guid id, JobApplication jobApplication);
		Task<JobApplication> DeleteJobApplication(Guid id);
        Task<IEnumerable<JobApplication>> GetEmployeeJobAppList(Guid employeeId);
        Task<IEnumerable<JobApplication>> GetEmployerJobAppList(Guid employerId);
    }
}


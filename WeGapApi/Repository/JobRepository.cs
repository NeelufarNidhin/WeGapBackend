using System;
using Microsoft.EntityFrameworkCore;
using WeGapApi.Data;
using WeGapApi.Models;
using WeGapApi.Models.Dto;
using WeGapApi.Repository.Interface;

namespace WeGapApi.Repository
{
	public class JobRepository : IJobRepository
	{
        private readonly ApplicationDbContext _context;
		public JobRepository(ApplicationDbContext context)
		{
            _context = context;
		}

        public async Task<Job> AddJobsAsync(Job job)
        {
            try
            {
                // Add job to context
                await _context.Jobs.AddAsync(job);

                
                await _context.SaveChangesAsync();
                return job;
              
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // Log or throw the exception as needed
                throw new Exception("Failed to add job with job skills.", ex);
            }
        }

       

        public async Task<Job> DeleteJobsAsync(Guid id)
        {
            var jobfromDb = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);
            if(jobfromDb is null)
            {
                throw new InvalidOperationException("Job Not found");
            }

            _context.Jobs.Remove(jobfromDb);

            await _context.SaveChangesAsync();

            return jobfromDb;
        }

        public async Task<List<Job>> GetAllJobsAsync()
        {
            return await _context.Jobs.Include(j=>j.Employer).ToListAsync();
        }

        public async Task<List<Job>> GetJobsByEmployerId(Guid id)
        {
            var job =  _context.Jobs.Where(u => u.EmployerId == id).ToList();
            return job;
        }

        public async Task<Job> GetJobsByIdAsync(Guid id)
        {
            var jobfromDb = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);
            if (jobfromDb is null)
            {
                throw new InvalidOperationException("Job Not found");
            }
            return jobfromDb;
        }

       

        public async Task<Job> UpdateJobsAsync(Guid id, Job job)
        {
           var jobfromDb = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id);

            if(jobfromDb == null)
            {
                throw new InvalidOperationException("Job Not found");
            }

            jobfromDb.JobTitle = job.JobTitle;
            jobfromDb.Description = job.Description;
            _context.Jobs.Update(jobfromDb);

           await _context.SaveChangesAsync();

            return jobfromDb;
        }

        public async Task <List<Job>> GetSearchQuery(string searchString)
        {
            var jobs = _context.Jobs.Where(x => x.Employer.CompanyName.ToLower().Contains(searchString)
            || x.JobTitle.ToLower().Contains(searchString) || x.Experience.ToLower().Contains(searchString)).ToList();

            return  jobs;
        }
    }
}


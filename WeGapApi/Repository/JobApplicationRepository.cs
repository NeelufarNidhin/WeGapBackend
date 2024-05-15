using System;
using WeGapApi.Data;
using WeGapApi.Models;
using WeGapApi.Repository.Interface;

namespace WeGapApi.Repository
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication)
        {
            try
            {
                await _context.JobApplications.AddAsync(jobApplication);
                await _context.SaveChangesAsync();
                return jobApplication;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to add job application", ex);
            }
        }

        public async Task<JobApplication> DeleteJobApplication(Guid id)
        {
            var jobApplicationFromDb = _context.JobApplications.FirstOrDefault(x => x.Id == id);

            if (jobApplicationFromDb is null)
            {
                throw new InvalidOperationException("job application not found");
            }

            _context.JobApplications.Remove(jobApplicationFromDb);
            await _context.SaveChangesAsync();
            return jobApplicationFromDb;
        }

        public async Task<IEnumerable<JobApplication>> GetAllJobApplicationAsync()
        {
            var jobApplications = _context.JobApplications.ToList();
            return jobApplications;
        }

        public async Task<JobApplication> GetJobApplicationById(Guid id)
        {
            var jobApplicationFromDb = _context.JobApplications.FirstOrDefault(x => x.Id == id);

            if (jobApplicationFromDb is null)
            {
                throw new InvalidOperationException("job application not found");
            }

            return jobApplicationFromDb;

        }

        public async Task<JobApplication> UpdateJobApplication(Guid id, JobApplication jobApplication)
        {
            var jobApplicationFromDb = _context.JobApplications.FirstOrDefault(x => x.Id == id);

            if (jobApplicationFromDb is null)
            {
                throw new InvalidOperationException("job application not found");
            }

            jobApplicationFromDb.JobStatus = jobApplication.JobStatus;
            //jobApplicationFromDb.Availability = jobApplication.Availability;
            //jobApplicationFromDb.CoverLetter = jobApplication.CoverLetter;
            //jobApplicationFromDb.ResumeFileName = jobApplication.ResumeFileName;

            _context.JobApplications.Update(jobApplicationFromDb);
            _context.SaveChanges();
            return jobApplicationFromDb;
        }


        public async Task<IEnumerable<JobApplication>> GetEmployeeJobAppList(Guid employeeId)
        {

            var jobApplications =  _context.JobApplications.Where(u => u.EmployeeId == employeeId).ToList();
            return jobApplications;

        }

        public async Task<IEnumerable<JobApplication>> GetEmployerJobAppList(Guid employerId)
        {

            var jobApplications = _context.JobApplications.Where(u => u.Employer == employerId).ToList();
            return jobApplications;

        }
    }
}


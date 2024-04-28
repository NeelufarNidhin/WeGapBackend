using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeGapApi.Data;
using WeGapApi.Models;
using WeGapApi.Models.Dto;
using WeGapApi.Repository;
using WeGapApi.Repository.Interface;
using WeGapApi.Services.Services.Interface;

namespace WeGapApi.Services
{
	public class JobService: IJobService
	{
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public JobService(IRepositoryManager repositoryManager, IMapper mapper, ApplicationDbContext context)
		{
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<JobDto> AddJobsAsync(AddJobDto addJobDto)
        {


            var job = _mapper.Map<Job>(addJobDto);
            var jobDomain = await _repositoryManager.Job.AddJobsAsync(job);

          //  Handle Job Skills association
            if (addJobDto.JobSkill != null && addJobDto.JobSkill.Any())
            {
                foreach (var skillId in addJobDto.JobSkill)
                {

                    var jobSkill = await _repositoryManager.JobSkill.GetJobSkillByIdAsync(skillId);
                    //if (jobSkill != null)
                    //{

                    JobJobSkill jobJobSkill = new JobJobSkill { JobId = job.Id, JobSkillId = skillId };

                    job.JobJobSkill.Add(jobJobSkill);


                    //}
                }
            }
            // new JobJobSkill { JobId = new Guid(), JobSkillId = new Guid() };
            await _context.SaveChangesAsync();

            var jobDto = _mapper.Map<JobDto>(jobDomain);

            return jobDto;
        }


        public async Task<JobDto> DeleteJobsAsync(Guid id)
        {
            var jobDomain = await _repositoryManager.Job.DeleteJobsAsync(id);



            var jobDto = _mapper.Map<JobDto>(jobDomain);
            return jobDto;
        }


        //public async Task<JobDto> DeleteJobsAsync(Guid id)
        //{
        //    // Obtain existing job entity
        //    var existingJob = await _repositoryManager.Job.GetJobsByIdAsync(id);

        //    // Remove job job skills
        //    existingJob.JobJobSkill.Clear();

        //    // Delete job from the repository
        //    var deletedJob = await _repositoryManager.Job.DeleteJobsAsync(id);

        //    // Map and return deleted job DTO
        //    var deletedJobDto = _mapper.Map<JobDto>(deletedJob);
        //    return deletedJobDto;
        //}

        public async Task<List<JobDto>> GetAllJobsAsync(int pageNumber, int pageSize)
        {
            var jobDomain = await _repositoryManager.Job.GetAllJobsAsync();
            var paginatedUsers = jobDomain.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var jobDto = _mapper.Map<List<JobDto>>(paginatedUsers);
            return jobDto;
        }

        public async Task<List<JobDto>> GetTotalJobs()
        {
            var jobDomain = await _repositoryManager.Job.GetAllJobsAsync();

            var jobDto = _mapper.Map<List<JobDto>>(jobDomain);
            return jobDto;
        }

        public async Task<List<JobSkillDto>> GetJobJobSkillsByIdAsync(Guid jobId)
        {
            var jobJobSkills = await _context.JobJobSkill
                                              .Where(jjs => jjs.JobId == jobId)
                                              .Include(jjs => jjs.JobSkill)
                                              .ToListAsync();

            // Map JobJobSkill entities to JobSkillDto
            var jobSkillDtos = jobJobSkills.Select(jjs => _mapper.Map<JobSkillDto>(jjs.JobSkill)).ToList();

            return jobSkillDtos;
        }

        public async Task<JobDto> GetJobsByIdAsync(Guid id)
        {
            //obtain data
            var jobDomain = await _repositoryManager.Job.GetJobsByIdAsync(id);

            

            //mapping
            var jobDto = _mapper.Map<JobDto>(jobDomain);
            return jobDto;

        }

        public async Task<List<JobDto>> GetJobsByEmployerId(Guid id)
        {
            //obtain data
            var jobDomain = await _repositoryManager.Job.GetJobsByEmployerId(id);



            //mapping
            var jobDto = _mapper.Map<List<JobDto>>(jobDomain);
            return jobDto;

        }

        //public async Task<JobDto> UpdateJobsAsync(Guid id, UpdateJobDto updateJobDto)
        //{



        //    //Map DTO to Domain model
        //    var jobDomain = _mapper.Map<Job>(updateJobDto);

        //    //check if employee exists
        //    jobDomain = await _repositoryManager.Job.UpdateJobsAsync(id, jobDomain);

        //    _context.SaveChanges();

        //    var jobDto = _mapper.Map<JobDto>(jobDomain);
        //    return jobDto;
        //}

        public async Task<List<JobDto>> GetSearchQuery(string searchString)
        {
            var jobs = await _repositoryManager.Job.GetSearchQuery(searchString);
            var jobDto = _mapper.Map<List<JobDto>>(jobs);
            return jobDto;
        }
        

        public async Task<List<JobDto>> GetJobType(string jobType)
        {
            var jobs = await _repositoryManager.Job.GetAllJobsAsync();
            var jobTypeFromDb = await _repositoryManager.JobType.GetJobTypeByName(jobType);

            jobs =  jobs.Where(x => x.JobTypeId ==  jobTypeFromDb.Id).ToList();
            var jobDto = _mapper.Map<List<JobDto>>(jobs);
            return jobDto;

        }

        public async Task<List<JobDto>> GetJobSkill(string jobSkill)
        {
            var jobs = await _repositoryManager.Job.GetAllJobsAsync();
            var jobSkillFromDb = await _repositoryManager.JobSkill.GetJobSkillByName(jobSkill);

            var jobIds = _context.JobJobSkill
                .Where(x => x.JobSkillId == jobSkillFromDb.Id)
                .Select(x => x.JobId)
                .ToList();

            jobs = jobs.Where(j => jobIds.Contains(j.Id)).ToList(); // Materialize the list here

            var jobDto = _mapper.Map<List<JobDto>>(jobs);
            return jobDto;

        }

        public async Task<JobDto> UpdateJobsAsync(Guid id, UpdateJobDto updateJobDto)
        {
            // Obtain existing job entity
            var existingJob = await _repositoryManager.Job.GetJobsByIdAsync(id);

            // Map DTO to existing domain model
            _mapper.Map(updateJobDto, existingJob);

            // Fetch existing job skills
            var existingJobSkills =  _context.JobJobSkill.Where(u=>u.JobId ==id);

            // Remove existing job skills that are not present in the update
            foreach (var existingJobSkill in existingJobSkills)
            {
                if (!updateJobDto.JobSkill.Contains(existingJobSkill.JobSkillId))
                {
                    existingJob.JobJobSkill.Remove(existingJobSkill);
                }
            }

            // Add/update job skills from the update
            foreach (var skillId in updateJobDto.JobSkill)
            {
                var jobSkill = await _repositoryManager.JobSkill.GetJobSkillByIdAsync(skillId);
                if (jobSkill != null)
                {
                    // Check if the job skill already exists in the job
                    if (!existingJob.JobJobSkill.Any(js => js.JobSkillId == skillId))
                    {
                        JobJobSkill jobJobSkill = new JobJobSkill { JobId = id, JobSkillId = skillId };
                        existingJob.JobJobSkill.Add(jobJobSkill);
                    }
                }
            }

            // Save changes to update job skills
            await _context.SaveChangesAsync();

            // Update job in the repository
            var updatedJob = await _repositoryManager.Job.UpdateJobsAsync(id, existingJob);

            // Map and return updated job DTO
            var updatedJobDto = _mapper.Map<JobDto>(updatedJob);
            return updatedJobDto;
        }
    }
    }


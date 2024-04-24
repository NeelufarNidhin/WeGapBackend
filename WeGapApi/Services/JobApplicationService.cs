using System;
using AutoMapper;
using WeGapApi.Models;
using WeGapApi.Models.Dto;
using WeGapApi.Repository.Interface;
using WeGapApi.Services.Services.Interface;

namespace WeGapApi.Services
{
	public class JobApplicationService : IJobApplicationService
	{
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
		public JobApplicationService(IRepositoryManager repository , IMapper mapper)
		{
            _repository = repository;
            _mapper = mapper;
		}

        public async Task<JobApplicationDto> CreateJobApplicationAsync(AddJobApplicationDto addJobApplicationDto)
        {
            var jobApplication = _mapper.Map<JobApplication>(addJobApplicationDto);
            var jobApplicationDomain = await _repository.JobApplication.CreateJobApplicationAsync(jobApplication);
            var jobApplicationDto = _mapper.Map<JobApplicationDto>(jobApplicationDomain);
            return jobApplicationDto;
        }

        public async Task<JobApplicationDto> DeleteJobApplication(Guid id)
        {
            var jobApplicationDomain = await _repository.JobApplication.DeleteJobApplication(id);
            var jobApplicationDto = _mapper.Map<JobApplicationDto>(jobApplicationDomain);
            return jobApplicationDto;
        }

        public async Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationAsync()
        {
            var jobApplicationDomain = await _repository.JobApplication.GetAllJobApplicationAsync();
            var jobApplicationDto = _mapper.Map<IEnumerable<JobApplicationDto>>(jobApplicationDomain);
            return jobApplicationDto;

        }

        public async Task<JobApplicationDto> GetJobApplicationById(Guid id)
        {
            var jobApplicationDomain = await _repository.JobApplication.GetJobApplicationById(id);
            var jobApplicationDto = _mapper.Map<JobApplicationDto>(jobApplicationDomain);
            return jobApplicationDto;
        }

        public async Task<JobApplicationDto> UpdateJobApplication(Guid id, UpdateJobApplicationDto updateJobApplicationDto)
        {
            var jobApplication = _mapper.Map<JobApplication>(updateJobApplicationDto);
            var jobApplicationDomain = await _repository.JobApplication.UpdateJobApplication(id,jobApplication);
            var jobApplicationDto = _mapper.Map<JobApplicationDto>(jobApplicationDomain);
            return jobApplicationDto;
        }
    }
}


using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using WeGapApi.Models;
using WeGapApi.Services.Services.Interface;
using WeGapApi.Utility;
using System.Net;
using WeGapApi.Models.Dto;
using System.Text.Json;

namespace WeGapApi.Controllers
{
    [Route("api/[controller]")]
    public class JobApplicationController : Controller
    {
        private readonly IServiceManager _service;
        private readonly ApiResponse _response;
        private readonly IBlobService _blobService;
        public JobApplicationController(IServiceManager service, IBlobService blobService)
        {
            _service = service;
            _response = new ApiResponse();
            _blobService = blobService;
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Employer + " ," + SD.Role_Employee)]
        public async Task<IActionResult> GetAllJobApplication()
        {
            try
            {
                var jobApplicationDto = await _service.JobApplicationService.GetAllJobApplicationAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = jobApplicationDto;
                return Ok(_response);

            }
            catch (InvalidOperationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }


        }
        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = SD.Role_Employer + " ," + SD.Role_Employee)]
        public async Task<IActionResult> GetEmployeeJobApplication([FromRoute] Guid employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                var totalJobApps = await _service.JobApplicationService.GetTotalEmployeeJobAppList(employeeId);
                var jobApplicationDto = await _service.JobApplicationService.GetEmployeeJobAppList(employeeId,pageNumber,pageSize);
                Pagination pagination = new Pagination
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalJobApps.Count()
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = jobApplicationDto;
                return Ok(_response);

            }
            catch (InvalidOperationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }

        }


        [HttpGet("employer/{employerId}")]
        [Authorize(Roles = SD.Role_Employer )]
        public async Task<IActionResult> GetEmployerJobApplication([FromRoute]Guid employerId, [FromQuery] int pageNumber=1, [FromQuery] int pagesize = 5)
        {
            try
            {
                var totalJobApps = await _service.JobApplicationService.GetTotalEmployerJobAppList(employerId);
                var jobApplicationDto = await _service.JobApplicationService.GetEmployerJobAppList(employerId,pageNumber,pagesize);

                Pagination pagination = new Pagination
                {
                    CurrentPage = pageNumber,
                    PageSize = pagesize,
                    TotalRecords = totalJobApps.Count()
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = jobApplicationDto;
                return Ok(_response);

            }
            catch (InvalidOperationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }


        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = SD.Role_Employer + " ," + SD.Role_Employee)]
        public async Task<IActionResult> GetJobApplicationById(Guid id)
        {
            try
            {
                var jobApplicationDto = await _service.JobApplicationService.GetJobApplicationById(id);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = jobApplicationDto;
                return Ok(_response);

            }
            catch (InvalidOperationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Employer + " ," + SD.Role_Employee)]
        public async Task<IActionResult> CreateJobApplication([FromForm] AddJobApplicationDto addJobApplicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if ( addJobApplicationDto.Resume  == null || addJobApplicationDto.Resume.Length == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(addJobApplicationDto.Resume.FileName)}";
                addJobApplicationDto.ResumeFileName = await _blobService.UploadBlob(fileName, SD.Storage_Container, addJobApplicationDto.Resume);


                var jobApplicationDto = await _service.JobApplicationService.CreateJobApplicationAsync(addJobApplicationDto);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = jobApplicationDto;
                return CreatedAtAction(nameof(GetJobApplicationById), new { id = jobApplicationDto.Id }, _response);
            }
            catch (InvalidOperationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }


        }

        [HttpPut("{id}")]
        [Authorize(Roles = SD.Role_Employer)]
        public async Task<IActionResult> UpdateJobApplication(Guid id,[FromBody] UpdateJobApplicationDto updateJobApplicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                //var jobApplicationFromDb = await _service.JobApplicationService.GetJobApplicationById(id);

                //if (jobApplicationFromDb.ResumeFileName != null || jobApplicationFromDb.ResumeFileName.Length > 0)
                //{
                //    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(updateJobApplicationDto.Resume.FileName)}";
                //    await _blobService.DeleteBlob(jobApplicationFromDb.ResumeFileName.Split('/').Last(), SD.Storage_Container);
                //    jobApplicationFromDb.ResumeFileName = await _blobService.UploadBlob(fileName, SD.Storage_Container, updateJobApplicationDto.Resume);
                //    updateJobApplicationDto.ResumeFileName = await _blobService.UploadBlob(fileName, SD.Storage_Container, updateJobApplicationDto.Resume);


                //}

                 var jobApplicationDto = await _service.JobApplicationService.UpdateJobApplication(id, updateJobApplicationDto);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = jobApplicationDto;
                return Ok(_response);

            }
            catch (InvalidOperationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = SD.Role_Employer)]
        public async Task<IActionResult> DeleteJobApplication(Guid id)
        {
            try{

                var jobApplicationFromDb = await _service.JobApplicationService.GetJobApplicationById(id);
                await _blobService.DeleteBlob(jobApplicationFromDb.ResumeFileName.Split('/').Last(), SD.Storage_Container);
                var jobApplicationDto = await _service.JobApplicationService.DeleteJobApplication(id);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = jobApplicationDto;
                return Ok(_response);

            }
            catch (InvalidOperationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }
            catch (Exception ex)
            {

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }


        }





    }
}


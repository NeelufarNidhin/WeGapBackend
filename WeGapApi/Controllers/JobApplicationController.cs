using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using WeGapApi.Models;
using WeGapApi.Services.Services.Interface;
using WeGapApi.Utility;
using System.Net;
using WeGapApi.Models.Dto;

namespace WeGapApi.Controllers
{
    [Route("api/[controller]")]
    public class JobApplicationController : Controller
    {
        private readonly IServiceManager _service;
        private readonly ApiResponse _response;
        public JobApplicationController(IServiceManager service)
        {
            _service = service;
            _response = new ApiResponse();
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
        public async Task<IActionResult> CreateJobApplication([FromBody] AddJobApplicationDto addJobApplicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
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
                var jobApplicationDto = _service.JobApplicationService.DeleteJobApplication(id);
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


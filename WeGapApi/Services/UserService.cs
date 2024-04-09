using System;
using AutoMapper;
using WeGapApi.Models;
using WeGapApi.Models.Dto;
using WeGapApi.Repository.Interface;
using WeGapApi.Services.Services.Interface;

namespace WeGapApi.Services
{
	public class UserService : IUserService
	{
		private readonly IRepositoryManager _repository;
		private readonly IMapper _mapper;
		public UserService(IRepositoryManager repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

        public async Task<UserDto> BlockUnblock(string id)
        {

            var user =  _repository.User.BlockUnblock(id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
           
        }

        public async Task<UserDto> DeleteUser(string id)
        {
            var user =  _repository.User.DeleteUser(id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<List<UserDto>> GetAllUsers(int pageNumber, int pageSize)
        {


            var users = _repository.User.GetUsers();
            var paginatedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var userDto = _mapper.Map<List<UserDto>>(paginatedUsers);
            return userDto;
        }

        public async Task<List<UserDto>> GetTotalUsers()
        {


            var users = _repository.User.GetUsers();
           

            var userDto = _mapper.Map<List<UserDto>>(users);
            return userDto;
        }


        public async Task<List<UserDto>> GetSearchQuery(string searchString)
        {
            var users = _repository.User.GetSearchQuery(searchString);
            var userDto = _mapper.Map<List<UserDto>>(users);
            return userDto;

        }

        public async Task<List<UserDto>> GetRole(string userRole)
        {
            var users = _repository.User.GetUsers();
            users = users.Where(u => u.Role == userRole).ToList();
            var userDto = _mapper.Map<List<UserDto>>(users);
            return userDto;

        }

        public async Task<UserDto> GetUserById(string id)
        {
            var user = _repository.User.GetUserById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;

        }

        public async Task<UserDto> UpdateUser(string id, UpdateUserDto updateUserDto)
        {
            var userDomain = _mapper.Map<ApplicationUser>(updateUserDto);
            var user = _repository.User.UpdateUser(id,userDomain);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}


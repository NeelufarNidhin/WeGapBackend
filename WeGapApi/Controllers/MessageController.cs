using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WeGapApi.Chats;
using WeGapApi.Data;
using WeGapApi.Models;
using WeGapApi.Repository.Interface;
using WeGapApi.Services.Services.Interface;
using WeGapApi.Utility;

namespace WeGapApi.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
	{
        private readonly ApplicationDbContext _context;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        public MessageController(ApplicationDbContext context, IMessageRepository messageRepository,IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
        }


        [Authorize(Roles = SD.Role_Employer + " ," + SD.Role_Employee)]
        [HttpGet("{senderId}/{receiverId}")]
        public async Task<IActionResult> GetMessages(string senderId, string receiverId)
        {
            try
            {
                var messages = await _messageRepository.GetMessagesBetween(senderId, receiverId);

                if (messages.Any())
                {
                    await _hubContext.Clients.User(receiverId).SendAsync("ReceiveNotification", "New Message", "You have received a new message.");
                }
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}


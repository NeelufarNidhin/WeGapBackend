using System;
using Microsoft.EntityFrameworkCore;
using WeGapApi.Data;
using WeGapApi.Models;
using WeGapApi.Repository.Interface;

namespace WeGapApi.Repository
{
	public class MessageRepository : IMessageRepository
	{
        private readonly ApplicationDbContext _context;
        public MessageRepository(ApplicationDbContext context)
		{
            _context = context;
		}

        public async Task<IEnumerable<ChatMessage>> GetMessagesBetween(string senderId, string receiverId)
        {
            return await _context.ChatMessages
                .Where(m => (m.Sender == senderId && m.Receiver == receiverId) || (m.Sender == receiverId && m.Receiver == senderId)).ToListAsync();


        }
       
    }
}


using System;
using WeGapApi.Models;

namespace WeGapApi.Repository.Interface
{
	public interface IMessageRepository
	{
        Task<IEnumerable<ChatMessage>> GetMessagesBetween(string senderId, string receiverId);
        //Task<ChatMessage> AddMessage(ChatMessage message);
        //      Task<ChatMessage> GetMessage(Guid id);
    }
}


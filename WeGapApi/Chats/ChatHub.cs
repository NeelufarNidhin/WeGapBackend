using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WeGapApi.Data;
using WeGapApi.Models;

namespace WeGapApi.Chats
{
    //[Authorize]
    public class ChatHub : Hub
	{
        private readonly ApplicationDbContext _context;
        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string sender,string receiver, string message)
        {
            var chatMessage = new ChatMessage
            {
                Sender = sender,
                Receiver = receiver,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Broadcast the message
          
            await Clients.User(receiver).SendAsync("ReceiveMessage", sender, message, DateTime.Now.ToShortTimeString());

            // Send notification
           

            await Clients.User(receiver).SendAsync("ReceiveNotification", "New Message", "You have received a new message."); // Notification to receiver
            await Clients.User(sender).SendAsync("ReceiveNotification", "Message Sent", "Your message was sent successfully."); // Notification to sender
        }
    }


    }



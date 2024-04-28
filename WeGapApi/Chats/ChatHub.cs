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

           
          
            await Clients.All.SendAsync("ReceiveMessage", sender, message, DateTime.Now.ToShortTimeString());

           // await Clients.User(receiver).SendAsync("ReceiveMessage", sender, message, DateTime.Now.ToShortTimeString());


            await Clients.User(receiver).SendAsync("ReceiveNotification", "New Message", "You have received a new message."); // Notification to receiver
            await Clients.User(sender).SendAsync("ReceiveNotification", "Message Sent", "Your message was sent successfully."); // Notification to sender
        }

        //public async Task StartVideoCall(string caller, string callee)
        //{
        //    // Send a signal to the callee to initiate the video call
        //    await Clients.User(callee).SendAsync("IncomingVideoCall", caller);

        //    // Notify the caller that the call is initiated
        //    await Clients.User(caller).SendAsync("VideoCallInitiated", callee);
        //}

        //public async Task AcceptVideoCall(string caller, string callee)
        //{
        //    // Notify both parties that the call is accepted
        //    await Clients.User(callee).SendAsync("VideoCallAccepted", caller);
        //    await Clients.User(caller).SendAsync("VideoCallAccepted", callee);
        //}

        //public async Task RejectVideoCall(string caller, string callee)
        //{
        //    // Notify the caller that the call is rejected
        //    await Clients.User(caller).SendAsync("VideoCallRejected", callee);
        //}
    }


    }



﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SignalRSample_Identity.Data;
using System.CodeDom;
using System.Security.Claims;

namespace SignalRSample_Identity.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;

        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }
        public override Task OnConnectedAsync()
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(UserId))
            {
                var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", UserId, userName);

                HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (HubConnections.HasUserConnection(UserId, Context.ConnectionId))
            {
                var UserConnections = HubConnections.Users[UserId];
                UserConnections.Remove(Context.ConnectionId);
                HubConnections.Users.Remove(UserId);

                if (UserConnections.Any())
                    HubConnections.Users.Add(UserId, UserConnections);

            }
            if (!String.IsNullOrEmpty(UserId))
            {
                var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisConnected", UserId, userName);

                HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SenAddRoomMessage(int maxRoom, int roomId, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

            await Clients.All.SendAsync("ReceiveAddRoomMessage", maxRoom, roomId, roomName, UserId, userName);
        }
        public async Task SendDeleteRoomMessage(int deleted, int selected, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

            await Clients.All.SendAsync("ReceiveDeleteRoomMessage", deleted, selected, roomName, userName);
        }

        public async Task SendPublicMessage(int roomId, string message, string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

            await Clients.All.SendAsync("ReceivePublicMessage", roomId, UserId, userName, message, roomName);
        }
        public async Task SendPrivateMessage(string receiverId, string message, string receiverName)
        {
            var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u => u.Id == senderId).UserName;
            
            var users = new string[] { senderId, receiverId };

            await Clients.Users(users).SendAsync("ReceivePrivateMessage", senderId, senderName,receiverId, message, Guid.NewGuid(), receiverName);
        }

        public async Task SendOpenPrivateChat(string receiverId)
        {
            var username = Context.User.FindFirstValue(ClaimTypes.Name);
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await Clients.User(receiverId).SendAsync("ReceiveOpenPrivateChat",userId,username);
        }
        public async Task SendDeletePrivateChat(string chatId)
        {
            await Clients.All.SendAsync("ReceiveDeletePrivateChat", chatId);
        }

        //public async Task SendMessageToAll(string user,string message)
        //{
        //    await Clients.All.SendAsync("MessageReceived", user, message);
        //}
        //[Authorize]
        //public async Task SendMessageToReceiver(string sender,string receiver, string message)
        //{
        //    var userId = _db.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower()).Id;
        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        await Clients.User(userId).SendAsync("MessageReceived", sender, message);
        //    }

        //}
    }
}

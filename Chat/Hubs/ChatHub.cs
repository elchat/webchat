using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Data;
using Chat.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using WebChat.Models;

namespace Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _context;
        public static List<ChatUser> Users = new List<ChatUser>();
        
        public ChatHub()
        {
            _context = new ApplicationContext();
        }

        public void Connect()
        {
            string userName = Context.User.Identity.GetUserName();
            string id = Context.ConnectionId;
            var user = new ChatUser(id, userName);
            Users.Add(user);
            ChatMessage model = GetModel(ChatMessage.MessageType.JOIN, user.UserName, null);
            Clients.Caller.onConnected(model, Users);
            Clients.Others.onNewUserConnected(model);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var user = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                Users.Remove(user);
                ChatMessage model = GetModel(ChatMessage.MessageType.LEAVE, user.UserName, null);
                Clients.All.onUserDisconnected(model);
            }
            return base.OnDisconnected(stopCalled);
        }

        public void SendMessage(string message)
        {
            var user = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            string current = user.UserName;
            ChatMessage model = GetModel(ChatMessage.MessageType.MESSAGE, current, message);
            Clients.All.addMessage(model);
        }

        private ChatMessage GetModel(ChatMessage.MessageType type, string userName, string message)
        {
            ChatMessage model = new ChatMessage();
            model.DateTime = DateTime.Now;
            model.UserName = userName;
            model.Type = type.ToString();
            model.Message = message;
            _context.ChatMessages.Add(model);
            _context.SaveChanges();
            return model;
        }
    }
}
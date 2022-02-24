using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Controllers;
using Chat.Models;
using Microsoft.AspNet.SignalR;
using WebChat.Models;

namespace Chat.Hubs
{
    public class ChatHub : Hub
    {
        private List<MessageViewModel> _messages = ChatController.Messages;

        public static List<ChatUser> Users = new List<ChatUser>();
        private static int _id = 0;

        public void Connect(string userName)
        {
            string id = Context.ConnectionId;
            var user = new ChatUser(id, userName);
            Users.Add(user);
            MessageViewModel model = GetModel(MessageViewModel.MessageType.JOIN, user.UserName, null);
            Clients.Caller.onConnected(model, Users);
            Clients.Others.onNewUserConnected(model);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var user = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                Users.Remove(user);
                MessageViewModel model = GetModel(MessageViewModel.MessageType.LEAVE, user.UserName, null);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(model);
            }
            return base.OnDisconnected(stopCalled);
        }

        public void SendMessage(string message)
        {
            var user = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            string current = user.UserName;
            MessageViewModel model = GetModel(MessageViewModel.MessageType.MESSAGE, current, message);
            Clients.All.addMessage(model);
        }

        private MessageViewModel GetModel(MessageViewModel.MessageType type, string userName, string message)
        {
            MessageViewModel model = new MessageViewModel();
            model.Id = ++_id;
            model.DateTime = DateTime.Now;
            model.UserName = userName;
            model.Type = type.ToString();
            model.Message = message;
            _messages.Add(model);
            return model;
        }
    }
}
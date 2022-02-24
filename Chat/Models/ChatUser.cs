using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class ChatUser
    {        
        public string ConnectionId { get; set; }
        public string UserName { get; set; }

        public ChatUser(string connectionId, string userName)
        {
            ConnectionId = connectionId;
            UserName = userName;
        }
    }
}
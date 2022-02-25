using System;

namespace Chat.Models
{
    public class ChatMessage
    {
        public enum MessageType
        {
            JOIN,
            LEAVE,
            MESSAGE
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public string UserName { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }
    }
}
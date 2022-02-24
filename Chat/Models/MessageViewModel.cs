using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chat.Models
{
    public class MessageViewModel
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
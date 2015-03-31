using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.Chat.Models
{
    public class MessageViewModel
    {
        public string Text { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
    }
}
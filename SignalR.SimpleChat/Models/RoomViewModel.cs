using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace SignalR.Chat.Models
{
    public class RoomViewModel
    {
        public Room Room { get; set; }

        public User Owner { get; set; }

        public User CurrentUser { get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public List<string> OnlineUserList { get; set; }
    }
}

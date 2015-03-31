using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SignalR.Chat.Models
{
    public class UserConnIdRoomId
    {
        public string ConnIdString { get; set; }
        public Guid RoomId { get; set; }
    }

    public class UserOnline
    {
        public Guid UserId { get; set; }
        //public Guid RoomId { get; set; }

        public DateTime LastResponse { get; set; }
        public List<UserConnIdRoomId> ConnIdRoomIdPairs { get; set; }
    }
}
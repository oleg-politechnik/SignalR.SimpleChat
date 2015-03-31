using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SignalR.Chat.Models
{
    public class Message
    {
        public Message()
        {
            Timestamp = DateTime.Now;
        }

        [Required]
        public Guid MessageId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        //TODO attachment

        [ScaffoldColumn(false)]
        public DateTime Timestamp { get; set; }

        public Guid RoomId { get; set; }
    }
}
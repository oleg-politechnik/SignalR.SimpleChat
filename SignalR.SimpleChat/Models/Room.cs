using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace SignalR.Chat.Models
{
    public class Room
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}

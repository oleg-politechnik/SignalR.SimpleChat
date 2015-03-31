using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SignalR.Chat.Models
{
    public class User
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Nickname { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}

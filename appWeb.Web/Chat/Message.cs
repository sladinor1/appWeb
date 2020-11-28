using appWeb.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Chat
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }  

        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public virtual User Sender { get; set; }
    }
}

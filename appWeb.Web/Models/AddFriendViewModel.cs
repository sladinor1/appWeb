using appWeb.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Models
{
    public class AddFriendViewModel
    {
        public string ToId { get; set; }

        public string FromId { get; set; }

        public User UserTo { get; set; }

        public User UserFrom { get; set; }

        [Display(Name = "User Name")]
        public string NameToFind { get; set; }
    }
}

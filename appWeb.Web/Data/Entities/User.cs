using appWeb.Common.Entities;
using appWeb.Web.Chat;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Data.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            Messages = new HashSet<Message>();
        }

        public virtual ICollection<Message> Messages { get; set; } 

        [MaxLength(20)]
        [Required]
        public string Document { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        public List<User> Friends { get; set; }

        public string Zelda { get; set; }

        [Display(Name = "User Type")]
        public UserType UserType { get; set; }

        public static implicit operator User(IdentityResult v)
        {
            throw new NotImplementedException();
        }

        //[Display(Name = "Image")]
        //public Guid ImageId { get; set; }

        ////TODO: Pending to put the correct paths
        //[Display(Name = "Image")]
        //public string ImageFullPath => ImageId == Guid.Empty
        //    ? $"https://localhost:44390/images/noimage.png"
        //    : $"https://onsale.blob.core.windows.net/users/{ImageId}";

    }
}

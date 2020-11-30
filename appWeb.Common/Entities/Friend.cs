using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Data.Entities
{
    public class Friend
    {
        public int Id { get; set; }

        [Required]
        public string FirstPersonId { get; set; }

        [Required]
        public string SecondPersonId { get; set; }

        public bool Relation { get; set; }
    }
}

using appWeb.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Tittle { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Price")]
        [MaxLength(12)]
        [RegularExpression(@"^\d+([\.\,]?\d+)?$", ErrorMessage = "Use only numbers and . or , to put decimals")]
        [Required]
        public string PriceString { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        public int Lot { get; set; }
    }
}

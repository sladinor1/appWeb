using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace appWeb.Common.Entities
{
    public class Product 
    {
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Tittle { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [NotMappedAttribute]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        [Display(Name = "Cantidad")]
        public int Lot { get; set; }

        //[DisplayName("Product Images Number")]
        //public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        //TODO: Pending to put the correct paths
        //[Display(Name = "Image")]
        //public ICollection<string> ImageFullPath { get; set; }
    }
}
using appWeb.Common.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace appWeb.Web.Models
{
    public class ProductCatalogyViewModel : Product
    {
        //public int Id { get; set; }

        //[Required]
        //public string Author { get; set; }

        //[Required]
        //public string Tittle { get; set; }

        //[DataType(DataType.MultilineText)]
        //public string Description { get; set; }

        //[DisplayFormat(DataFormatString = "{0:C2}")]
        //public decimal Price { get; set; }

        public List<CategoryViewModel> CategoryList { get; set; }

        [Display(Name = "Cantidad")]
        public int Lot { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        [DisplayName("Product Images Number")]
        public int ProductImagesNumber => ProductImages == null ? 0 : ProductImages.Count;

        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath { get; set; }
    }
}


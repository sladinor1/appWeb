using System;
using System.Collections.Generic;
using System.Text;

namespace appWeb.Common.Entities
{
    public class CategoryProduct
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }

        public int IdCategory { get; set; }
    }
}

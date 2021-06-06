using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Models.Products;

namespace clothing_store_api.Models
{
    public class Product
    {
        public int Id { get; set; }

        // ...photo for example
        public ProductImage[] Images { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public int Available { get; set; }
        public ProductMaterial Material { get; set; }
        public ProductType Type { get; set; }

        public ProductSize Size { get; set; }
    }
}

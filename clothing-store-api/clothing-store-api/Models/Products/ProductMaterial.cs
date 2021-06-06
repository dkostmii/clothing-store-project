using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Models.Products
{
    public class MaterialRaw
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public decimal Percent { get; set; }
    }

    public class ProductMaterial
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public ProductColor Color { get; set; }
        public MaterialRaw[] Raw { get; set; }
    }
}

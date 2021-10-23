using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Models;
using clothing_store_api.Models.Base;

namespace clothing_store_api.Types
{
    public class ProductBuyDTO
    {
        public int Amount { get; set; }
        public int CustomerId { get; set; }
    }

    public class ProductResponse
    {
        public int Id { get; set; }
        public ICollection<Image> Images { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public int Available { get; set; }
        public Material Material { get; set; }
        public Models.Base.Type Type { get; set; }
        public Size Size { get; set; }
    }
}

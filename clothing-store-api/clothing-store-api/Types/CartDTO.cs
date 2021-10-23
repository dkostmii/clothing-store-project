using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Types
{
    public class CartDTO
    {
        public int Id { get; set; }
        public ProductResponse Product { get; set; }
        public int Quantity { get; set; }
    }
}

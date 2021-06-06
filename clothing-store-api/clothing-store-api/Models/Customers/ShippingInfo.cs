using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Models.Customers
{
    public class ShippingInfo
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string  PostalCode { get; set; }
        public string ContactPhone { get; set; }
    }
}

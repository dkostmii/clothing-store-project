using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Models.Products;
using clothing_store_api.Models.Customers;

namespace clothing_store_api.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ShippingInfo[] ShippingInfo { get;set; }
        public ProductSize[] PreferredSizes { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clothing_store_api.Models.Customers
{
    public class ShippingInfoDTO
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string ContactPhone { get; set; }
    }

    public class ShippingInfo : ShippingInfoDTO
    {
        public ShippingInfo() { }
        public ShippingInfo(ShippingInfoDTO data)
        {
            this.Country = data.Country;
            this.City = data.City;
            this.Address = data.Address;
            this.PostalCode = data.PostalCode;
            this.ContactPhone = data.ContactPhone;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
    }
}

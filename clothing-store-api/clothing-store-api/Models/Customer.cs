using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Models.Customers;
using clothing_store_api.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace clothing_store_api.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public User User { get; set; }

        [Column(TypeName = "SMALLMONEY")]
        public decimal Balance { get; set; }
        public ShippingInfo? ShippingInfo { get;set; }
        public ICollection<CartPosition> Cart { get; set; }
        [ForeignKey("CustomerId")]
        public ICollection<Order> Orders { get; set; }
    }
}
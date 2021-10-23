using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace clothing_store_api.Models.Customers
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public ShippingInfo ShippingInfo { get; set; }
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "SMALLMONEY")]
        public decimal OrderSummary { get; set; }
    }
}

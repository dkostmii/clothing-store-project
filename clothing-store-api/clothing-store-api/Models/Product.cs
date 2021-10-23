using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Models.Products;
using clothing_store_api.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace clothing_store_api.Models
{
    public class ProductDTO
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public decimal Price { get; set; }
        public int Available { get; set; }
        public int MaterialId { get; set; }
        public int TypeId { get; set; }
        public int SizeId { get; set; }
    }

  

    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }


        public ICollection<ProductImage> Images { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }

        [Column(TypeName = "SMALLMONEY")]
        public decimal Price { get; set; }
        public int Available { get; set; }
        public Material Material { get; set; }
        public Base.Type Type { get; set; }

        public Size Size { get; set; }
    }
}

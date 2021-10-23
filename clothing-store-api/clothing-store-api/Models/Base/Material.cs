using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Models.Base
{
    public class MaterialRaw
    {
        public int Id { get; set; }
        public Raw Raw { get; set; }

        [Column(TypeName = "DECIMAL(3, 1)")]
        public decimal Amount { get; set; } // 0.0% < Amount < 100.0%
     }
    public class Material
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Color Color { get; set; }
        public ICollection<MaterialRaw> Materials { get; set; }
    }
}

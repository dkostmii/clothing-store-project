using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace clothing_store_api.Types
{
    public class ColorDTO
    {
        public string? Name { get; set; }
        public string? HexCode { get; set; }
    }

    public class MaterialRawDTO
    {
        public int? RawId { get; set; }
        public decimal? Amount { get; set; }
    }

    public class MaterialDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ColorDTO Color { get; set; }
        public ICollection<MaterialRawDTO> MaterialRaws { get; set; }
    }
}

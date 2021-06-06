using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Models.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public long UI { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}

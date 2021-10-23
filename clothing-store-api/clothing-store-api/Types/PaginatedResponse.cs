using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


#nullable enable

namespace clothing_store_api.Types
{
    public class PaginatedResponse<T>
    {
        public int Count { get; set; }
        public ICollection<T>? Results { get; set; }
        public string? Next { get; set; }
        public string? Prev { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Models;

namespace clothing_store_api.Types
{
    public interface IJwtGenerator
    {
        string CreateToken(UserRole data);
    }
}

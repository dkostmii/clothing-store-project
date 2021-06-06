using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldControllers
    {
        [HttpGet]
        public String Get()
        {
            return "Hello, World!";  
        }
    }
}

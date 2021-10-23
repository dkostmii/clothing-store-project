using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Data;
using clothing_store_api.Models.Base;
using clothing_store_api.Types;
using clothing_store_api.Validators;

using Microsoft.EntityFrameworkCore;

using System.Net;
using Microsoft.Extensions.Configuration;

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("/")]
    public class MainController : Controller
    {
        private readonly string host;

        public MainController(IConfiguration config)
        {
            host = config.GetValue<string>("Host");
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Dictionary<string, string>
            {
                { "Helloworld", host + "/Helloworldcontrollers" },
                { "Auth", host + "/Auth" },
                { "Users", host + "/Users" },
                { "Customer", host + "/Customer" },
                { "Products", host + "/Products" },
                { "ProductImages", host + "/Products/Images" },
                { "ProductSizes", host + "/Products/Sizes" },
                { "ProductTypes", host + "/Products/Types" },
                { "ProductColors", host + "/Products/Colors" },
                { "ProductRaws", host + "/Products/Raws" },
                { "ProductMaterials", host + "/Products/Materials" },
            });
        }
    }
}

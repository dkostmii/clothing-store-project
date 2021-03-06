using clothing_store_api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Models;
using clothing_store_api.Models.Products;
using clothing_store_api.Models.Base;

using clothing_store_api.Validators;

using clothing_store_api.Types;
using clothing_store_api.Middlewares;

using Microsoft.Extensions.Logging;

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("Products/Sizes")]
    public class ProductSizesController : Controller
    {
        private ManagerContext _managerContext;
        private CustomerContext _customerContext;

        private readonly string host;

        private ILogger<ProductImagesController> _logger;

        private Auth _auth;

        public ProductSizesController(ILogger<ProductImagesController> logger, IConfiguration config, 
                                      CustomerContext customerContext, ManagerContext managerContext, Auth auth)
        {
            _customerContext = customerContext;
            _managerContext = managerContext;

            host = config.GetValue<string>("Host");

            _logger = logger;

            _auth = auth;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new { Count = _customerContext.Sizes.ToList().Count(), ProductSizes = _customerContext.Sizes.ToList() });
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var found = _customerContext.Sizes.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Size with id: {id} not found" });
            }

            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string name)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);

            if (!UserDataValidator.ValidateRequired(name))
            {
                return BadRequest(new { Message = "Name is required" });
            }

            var newSize = new Size { Name = name };

            _managerContext.Sizes.Add(newSize);

            await _managerContext.SaveChangesAsync();

            return Ok(new { Message = "Success!", Size = newSize });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);

            var found = _customerContext.Sizes.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Size with id: {id} not found" });
            }

            _managerContext.Sizes.Remove(found);

            await _managerContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] string name)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);

            if (!UserDataValidator.ValidateRequired(name))
            {
                return BadRequest(new { Message = "Name is required" });
            }

            var found = _customerContext.Sizes.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Size with id: {id} not found" });
            }

            found.Name = name;

            await _managerContext.SaveChangesAsync();

            return Ok(new { Message = "Success!", Size = found });
        }
    }
}

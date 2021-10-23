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

using clothing_store_api.Types;
using clothing_store_api.Middlewares;

using clothing_store_api.Validators;

using Microsoft.Extensions.Logging;

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("Products/Types")]
    public class ProductTypesController : Controller
    {
        private ManagerContext _managerContext;
        private CustomerContext _customerContext;

        private readonly string host;

        private ILogger<ProductImagesController> _logger;

        private Auth _auth;

        public ProductTypesController(ILogger<ProductImagesController> logger, IConfiguration config,
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
            return Ok(new { Count = _customerContext.Types.ToList().Count(), ProductTypes = _customerContext.Types.ToList() });
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var found = _customerContext.Types.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Type with id: {id} not found" });
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

            var newType = new Models.Base.Type { Name = name };

            _managerContext.Types.Add(newType);

            await _managerContext.SaveChangesAsync();

            return Ok(new { Message = "Success!", Type = newType });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);

            var found = _customerContext.Types.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Type with id: {id} not found" });
            }

            _managerContext.Types.Remove(found);

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

            var found = _customerContext.Types.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Type with id: {id} not found" });
            }

            found.Name = name;

            await _managerContext.SaveChangesAsync();

            return Ok(new { Message = "Success!", Type = found });
        }
    }
}

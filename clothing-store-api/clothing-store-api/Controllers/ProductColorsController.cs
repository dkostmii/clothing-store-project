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

using clothing_store_api.Middlewares;

using clothing_store_api.Types;

using clothing_store_api.Validators;

using Microsoft.Extensions.Logging;

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("Products/Colors")]
    public class ProductColorsController : Controller
    {
        private ManagerContext _managerContext;
        private CustomerContext _customerContext;

        private readonly string host;

        private ILogger<ProductImagesController> _logger;

        private Auth _auth;

        public ProductColorsController(ILogger<ProductImagesController> logger, IConfiguration config,
                                      CustomerContext customerContext, ManagerContext managerContext, Auth auth)
        {
            _customerContext = customerContext;
            _managerContext = managerContext;

            host = config.GetValue<string>("Host");
            _auth = auth;

            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new { Count = _customerContext.Colors.ToList().Count(), ProductColors = _customerContext.Colors });
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var found = _customerContext.Colors.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Color with id: {id} not found" });
            }

            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ColorDTO data)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);
            if (!UserDataValidator.ValidateRequired(data.Name))
            {
                return BadRequest(new { Message = "Name is required" });
            }

            if (!UserDataValidator.ValidateRequired(data.HexCode))
            {
                return BadRequest(new { Message = "HexCode is required" });
            }

            if (!UserDataValidator.ValidateHexColor(data.HexCode))
            {
                return BadRequest(new { Message = "Invalid hex color code provided." });
            }

            var newColor = new Models.Base.Color { Name = data.Name, HexCode = data.HexCode };

            _managerContext.Colors.Add(newColor);

            await _managerContext.SaveChangesAsync();

            return Ok(new { Message = "Success!", Color = newColor });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);
            var found = _customerContext.Colors.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Color with id: {id} not found" });
            }

            _managerContext.Colors.Remove(found);

            await _managerContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] ColorDTO data)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);
            if (!UserDataValidator.ValidateHexColor(data.HexCode))
            {
                return BadRequest(new { Message = "Invalid hex color code provided." });
            }

            var found = _customerContext.Colors.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Color with id: {id} not found" });
            }

            if (data.Name != null && UserDataValidator.ValidateRequired(data.Name))
            {
                found.Name = data.Name;
            }

            if (data.HexCode != null && UserDataValidator.ValidateRequired(data.HexCode))
            {
                found.HexCode = data.HexCode;
            }

            await _managerContext.SaveChangesAsync();

            return Ok(new { Message = "Success!", Color = found });
        }
    }
}

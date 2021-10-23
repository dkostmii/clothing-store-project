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
    [Route("Products/Materials")]
    public class ProductMaterialsController : Controller
    {
        private ManagerContext _managerContext;
        private CustomerContext _customerContext;

        private readonly string host;

        private ILogger<ProductImagesController> _logger;

        private Validate _validate;

        private Auth _auth;

        public ProductMaterialsController(ILogger<ProductImagesController> logger, IConfiguration config,
                                      CustomerContext customerContext, ManagerContext managerContext,
                                      Validate validate, Auth auth)
        {
            _customerContext = customerContext;
            _managerContext = managerContext;

            _validate = validate;

            host = config.GetValue<string>("Host");

            _logger = logger;

            _auth = auth;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new { Count = _customerContext.Materials.ToList().Count(), ProductMaterials = _customerContext.Materials });
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var found = _customerContext.Materials.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Material with id: {id} not found" });
            }

            return Ok(found);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MaterialDTO data)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);

            if (!_validate.IsMaterial(data))
            {
                return BadRequest(new { Message = "Missing required fields" });
            }

            foreach (var materialRaw in data.MaterialRaws)
            {
                if (materialRaw.RawId == null || materialRaw.Amount == null)
                {
                    return BadRequest(new { Message = "Missing required fields" });
                }
            }

            var newColor = new Color
            {
                Name = data.Color.Name,
                HexCode = data.Color.HexCode
            };

            var newMaterialRaws = new List<MaterialRaw>();


            foreach (var materialRaw in data.MaterialRaws)
            {
                var foundRaw = _customerContext.Raws.Find(materialRaw.RawId);
                if (foundRaw == null)
                {
                    return BadRequest(new { Message = $"Provided Raw id: {materialRaw.RawId} not found."});
                }

                newMaterialRaws.Add(new MaterialRaw
                {
                    Raw = foundRaw,
                    Amount = (decimal) materialRaw.Amount,
                });
            } 

            var newMaterial = new Models.Base.Material
            {
                Title = data.Title,
                Description = data.Description,
                Color = newColor,
                Materials = newMaterialRaws
            };

            _managerContext.Materials.Add(newMaterial);

            await _managerContext.SaveChangesAsync();

            return Ok(new { Message = "Success!", Material = newMaterial });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            _auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);

            var found = _customerContext.Materials.Find(id);
            if (found == null)
            {
                return NotFound(new { Message = $"Product Material with id: {id} not found" });
            }

            _managerContext.Materials.Remove(found);

            await _managerContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

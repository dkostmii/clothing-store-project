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

using clothing_store_api.Types;
using clothing_store_api.Middlewares;

using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using clothing_store_api.Services;


namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("Products/Images")]
    public class ProductImagesController : Controller
    {
        private ManagerContext _context;
        private readonly string host;
        private readonly int maxUploadSize;

        private ILogger<ProductImagesController> _logger;

        private ImagesService _service;

        private Auth _auth;

        public ProductImagesController(ImagesService service, ILogger<ProductImagesController> logger,IConfiguration config, ManagerContext context, Auth auth)
        {
            _context = context;

            _service = service;

            host = config.GetValue<string>("Host");
            maxUploadSize = config.GetValue<int>("MaxImageUploadSize");

            _logger = logger;

            _auth = auth;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var image = _service.FetchImage(id);
                return File(image.ImageData, image.ImageMimeType);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post(IFormFile file, [FromQuery] int productId)
        {
            //_auth.AuthorizeByRole(Unauthorized, HttpContext, Role.Manager);

            _logger.LogInformation($"Uploading product image for productId: {productId}");

            if (file.Length > maxUploadSize)
            {
                _logger.LogError($"File is too large");
                return BadRequest(new { Message = "File is too large" });
            }

            try
            {
                _service.UploadImage(productId, file);
            }
            catch (Exception ex)
            {
                NotFound(new { Message = ex.Message });
            }

            return Ok(new { Message = "Successfully uploaded image!" });
        }
        

        [HttpGet]
        public IActionResult GetAll([FromQuery] int productId)
        {
            try
            {
                return Ok(_service.GetImages(productId));
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}

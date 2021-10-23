using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Data;
using clothing_store_api.Models.Base;
using clothing_store_api.Validators;

using clothing_store_api.Types;
using clothing_store_api.Middlewares;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using System.Net;

using Microsoft.Extensions.Configuration;
using clothing_store_api.Models;
using clothing_store_api.Models.Products;
using clothing_store_api.Services;


#nullable enable

namespace clothing_store_api.Controllers
{
    public class ProductQuery
    {
        public string? q { get; set; }
        public int? limit { get; set; }
        public int? offset { get; set; }
    }

    public class QuickOrderDTO
    {
        public int userId { get; set; }
        public int count { get; set; }
    }

    public class ProductCartDTO 
    {
        public int? UserId { get; set; }
        public int? Count { get; set; }
    }


    [ApiController]
    [Route("Products")]
    public class ProductsController : Controller
    {
        ILogger<ProductsController> _logger;

        private readonly string host;

        ManagerContext _managerContext;
        CustomerContext _customerContext;
        SystemContext _systemContext;


        private ProductsService _service;
        private CustomerService _customerService;

        private Auth _auth;

        public ProductsController(IConfiguration config,
                                  ProductsService service,
                                  CustomerService customerService,
                                  ILogger<ProductsController> logger,
                                  ManagerContext managerContext,
                                  CustomerContext customerContext,
                                  SystemContext systemContext,
                                  Auth auth)
        {
            _logger = logger;

            host = config.GetValue<string>("Host");

            _service = service;
            _customerService = customerService;

            _managerContext = managerContext;
            _auth = auth;

            _customerContext = customerContext;

            _systemContext = systemContext;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] ProductQuery queryData)
        {
            _logger.LogInformation("Got query data: " + JsonConvert.SerializeObject(queryData));

            int offset = 0;
            int limit = 20;

            if (queryData.offset != null)
                offset = (int)queryData.offset;
            if (queryData.limit != null)
                limit = (int)queryData.limit;

            var queryString = queryData.q != null ? "q=" + queryData.q + "&" : "";

            var resData = _service.GetAllProducts(queryString, offset, limit);
            return Ok(resData);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                return Ok(_service.GetProductResponse(_service.GetProduct(id)));
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductDTO data)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.Authorize(handler, HttpContext))
            {
                _logger.LogInformation("Got body: " + JsonConvert.SerializeObject(data));

                try
                {
                    var newProd = _service.CreateProduct(data);
                    return Ok(new { Message = "Success!", Product = newProd });
                }
                catch (Exception ex)
                {
                    return Conflict(new { Message = ex.Message });
                }
            }
            else
            {
                return handler(new { Message = "Authorization failed." });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Manager))
            {
                try
                {
                    _service.RemoveProduct(id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return NotFound(new { Message = ex.Message });
                }
            }
            else
            {
                return handler(new { Message = "Manager authorization failed." });
            }
        }

        [HttpPost]
        [Route("{id}/Order")]
        public IActionResult Order([FromRoute] int id, [FromBody] QuickOrderDTO data)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                _logger.LogInformation("Got body: " + JsonConvert.SerializeObject(data));
                try
                {
                    return Ok(_customerService.Order(data.userId, id, data.count));
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
            else
            {
                return handler(new { Message = "Authorization failed." });
            }
        }

        [HttpPost]
        [Route("{id}/Cart")]
        public IActionResult ToCart([FromRoute] int id, [FromBody] ProductCartDTO data)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                if (data.UserId == null)
                {
                    return BadRequest(new { Message = "No userId specified" });
                }

                var customer = _customerService.GetCustomer((int) data.UserId);

                return Ok(_customerService.AddToCart(customer.User.Id, id, data.Count != null ? (int) data.Count : 1));
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }
    }
}

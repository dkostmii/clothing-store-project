using clothing_store_api.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using clothing_store_api.Services;
using clothing_store_api.Middlewares;
using clothing_store_api.Models;
using clothing_store_api.Data;
using clothing_store_api.Models.Customers;

using Microsoft.Extensions.Configuration;

#nullable enable

namespace clothing_store_api.Controllers
{
    public class CartBody
    {
        public int? ProductId { get; set; }
        public int? Count { get; set; }
    }

    public class DeleteFromCartBody
    {
        public int? CartPositionId { get; set; }
    }

    [ApiController]
    [Route("Customer")]
    public class CustomerController : Controller
    {
        private ILogger<UserController> _logger;
        private Auth _auth;
        private SystemContext _context;
        private CustomerService _service;
        private ProductsService _productsService;
        private ManagerContext _managerContext;
        private CustomerContext _customerContext;

        private readonly string host;

        public CustomerController(
                        ILogger<UserController> logger,
                        CustomerService service,
                        ProductsService productsService,
                        Auth auth,
                        SystemContext context,
                        ManagerContext managerContext,
                        CustomerContext customerContext,
                        IConfiguration config
            )
        {
            _logger = logger;

            _auth = auth;

            _context = context;

            _service = service;
            _productsService = productsService;

            _managerContext = managerContext;
            _customerContext = customerContext;

            host = config.GetValue<string>("Host");
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRoutesMapping([FromRoute] int id)
        {
            return Ok(new Dictionary<string, string>
            {
                { "Orders", host + $"{id}/Orders" },
                { "ShippingInfo", host + $"{id}/ShippingInfo" },
                { "Cart", host + $"{id}/Cart" },
            });
        }

        [HttpGet]
        [Route("{id}/Orders")]
        public IActionResult GetOrders([FromRoute] int id)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                try
                {
                    var customer = _service.GetCustomer(id);

                    return Ok(customer.Orders);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }

        [HttpPost]
        [Route("{id}/ShippingInfo")]
        public IActionResult PostShippingInfo([FromRoute] int id, [FromBody] ShippingInfoDTO data)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                try
                {
                    var customer = _service.GetCustomer(id);

                    _service.AddShippingInfo(customer.User.Id, data);
                    return Ok(new { Message = "Successfully updated" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }

        [HttpGet]
        [Route("{id}/ShippingInfo")]
        public IActionResult GetShippingInfo([FromRoute] int id)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                var customer = _service.GetCustomer(id);

                if (customer.ShippingInfo != null)
                {
                    return Ok(customer.ShippingInfo);
                }

                return NoContent();
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }


        [HttpGet]
        [Route("{id}/Cart")]
        public IActionResult GetCart([FromRoute] int id)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                var customer = _service.GetCustomer(id);

                var cartResponse = customer.Cart.ToList().Select(position =>
                {
                    return new CartDTO
                    {
                        Id = position.Id,
                        Product = _productsService.GetProductResponse(position.Product),
                        Quantity = position.Quantity
                    };
                });

                return Ok(cartResponse);
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }

        [HttpGet]
        [Route("{id}/Balance")]
        public IActionResult GetBalance([FromRoute] int id)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                var customer = _service.GetCustomer(id);

                return Ok(new { Balance = customer.Balance });
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }



        [HttpDelete]
        [Route("{id}/Cart")]
        public IActionResult DeleteFromCart([FromRoute] int id, [FromBody] DeleteFromCartBody data)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                var customer = _service.GetCustomer(id);

                if (data.CartPositionId == null)
                {
                    throw new Exception("CartPositionId is not provided");
                }

                _service.RemoveFromCart(customer.User.Id, (int) data.CartPositionId);
                return NoContent();
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }

        [HttpDelete]
        [Route("{id}/Cart/Clear")]
        public IActionResult ClearCart([FromRoute] int id)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                var customer = _service.GetCustomer(id);

                _service.ClearCart(id);
                return NoContent();
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }

        [HttpPost]
        [Route("{id}/Cart")]
        public IActionResult CreateCartPosition([FromRoute] int id, [FromBody] CartBody data)
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.AuthorizeByRole(handler, HttpContext, Role.Customer))
            {
                if (data.ProductId == null)
                {
                    return BadRequest(new { Message = "No ProductId specified" });
                }

                var product = _productsService.GetProduct((int) data.ProductId);

                var customer = _service.GetCustomer(id);

                return Ok(_service.AddToCart(customer.User.Id, (int) data.ProductId, data.Count != null ? (int) data.Count : 1));
            }
            else
            {
                return handler(new { Message = "Customer Authorization failed." });
            }
        }
    }
}

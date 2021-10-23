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
using clothing_store_api.Services;

using Microsoft.EntityFrameworkCore;

using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : Controller
    {
        AuthService _service;
        ILogger<AuthController> _logger;

        private readonly string host;

        public AuthController(IConfiguration config, AuthService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;

            host = config.GetValue<string>("Host");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Dictionary<string, string>
            {
                { "Customers", host + "/Auth/Customers" },
                { "Managers", host + "/Auth/Managers" }
            });
        }

        [HttpGet]
        [Route("Customers")]
        public IActionResult GetCustomers()
        {
            return Ok(new Dictionary<string, string>
            {
                { "SignIn", host + "/Auth/Customers/SignIn" },
                { "SignUp", host + "/Auth/Customers/SignUp" }
            });
        }

        [HttpGet]
        [Route("Managers")]
        public IActionResult GetManagers()
        {
            return Ok(new Dictionary<string, string>
            {
                { "SignIn", host + "/Auth/Managers/SignIn" },
                { "SignUp", host + "/Auth/Managers/SignUp" }
            });
        }


        [HttpPost]
        [Route("Customers/SignIn")]
        public async Task<IActionResult> CustomerSignIn([FromBody] SignInDTO data)
        {
            return await SignIn(Role.Customer, data);
        }

        [HttpPost]
        [Route("Managers/SignIn")]
        public async Task<IActionResult> ManagerSignIn([FromBody] SignInDTO data)
        {
            return await SignIn(Role.Manager, data);
        }

        [HttpPost]
        [Route("Customers/SignUp")]
        public async Task<IActionResult> CustomerSignUp([FromBody] SignUpDTO data)
        {
            return await SignUp(Role.Customer, data);
        }

        [HttpPost]
        [Route("Managers/SignUp")]
        public async Task<IActionResult> ManagerSignUp([FromBody] SignUpDTO data)
        {
            return await SignUp(Role.Manager, data);
        }


        private async Task<IActionResult> SignIn(Role role, SignInDTO data)
        {
            _logger.LogInformation("[SignIn] Got body: " + JsonConvert.SerializeObject(data));

            if (!new SignInValidator(data).Validate())
            {
                return BadRequest(new { message = "Sign in data is not valid." });
            }

            AuthResult result = await _service.SignIn(new SignInRole { Role = role, User = data });

            return await Task.Run(() => StatusCode(result.Status, new { Message = result.Message, User = result.User, Token = result.Token }));
        }


        private async Task<IActionResult> SignUp(Role role, SignUpDTO data)
        {
            _logger.LogInformation("[SignUp] Got body: " + JsonConvert.SerializeObject(data));

            if (!new SignUpValidator(data).Validate())
            {
                return BadRequest(new { message = "Sign up data is not valid." });
            }

            AuthResult result = await _service.SignUp(new SignUpRole { Role = role, User = data });

            return await Task.Run(() => StatusCode(result.Status, new { Message = result.Message, User = result.User, Token = result.Token }));
        }
    }
}

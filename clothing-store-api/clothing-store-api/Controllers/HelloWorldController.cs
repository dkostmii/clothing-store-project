using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Data;
using clothing_store_api.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

using clothing_store_api.Types;
using clothing_store_api.Middlewares;

#nullable enable

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldControllers : Controller
    {
        private ILogger<HelloWorldControllers> _logger;
        private readonly TokenValidationParameters _params;
        private readonly IConfiguration _config;

        private readonly Auth _auth;
        public HelloWorldControllers(IConfiguration config,
                                ILogger<HelloWorldControllers> logger,
                                TokenValidationParameters parameters,
                                Auth auth)
        {
            _logger = logger;
            _params = parameters;
            _auth = auth;

            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.Authorize(handler, HttpContext))
            {
                return await Task.Run(() => Ok(new { Role = _auth.GetRole(_auth.GetToken(HttpContext)).Value }));
            }
            else
            {
                return await Task.Run(() => handler(new { Message = "Cannot obtain current user's role" }));
            }
        }
    }
}

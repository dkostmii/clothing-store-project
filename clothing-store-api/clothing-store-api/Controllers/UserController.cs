using clothing_store_api.Services;
using clothing_store_api.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Middlewares;
using clothing_store_api.Validators;

namespace clothing_store_api.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : Controller
    {
        AuthService _authService;
        ILogger<UserController> _logger;
        Auth _auth;

        public UserController(
                AuthService authService,
                ILogger<UserController> logger,
                Auth auth
            )
        {
            _logger = logger;
            _authService = authService;

            _auth = auth;
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] OptionalUserDTO data)
        {
            _logger.LogInformation("Put [Users/" + id + "] " + "Got body: " + JsonConvert.SerializeObject(data));

            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.Authorize(handler, HttpContext))
            {
                if (!(((!String.IsNullOrEmpty(data.Email) && UserDataValidator.ValidateEmail(data.Email)) || String.IsNullOrEmpty(data.Email)) &&
                    ((!String.IsNullOrEmpty(data.Phone) && UserDataValidator.ValidatePhone(data.Phone)) || String.IsNullOrEmpty(data.Phone)) &&
                    ((!String.IsNullOrEmpty(data.Password) && UserDataValidator.ValidatePassword(data.Password)) || String.IsNullOrEmpty(data.Password) &&
                    ((data.FirstName != null && data.FirstName.Length > 0) || data.FirstName == null)
                    ))

                    )
                {
                    return BadRequest(new { message = "New user data is not valid." });
                }

                try
                {
                    return Ok(
                        new
                        {
                            Message = "Successfully updated!",
                            User = _authService.UpdateUserAccount(id, data)
                        }
                      );
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

        [HttpDelete]
        [Route("{id})")]
        public IActionResult Delete([FromRoute] int id)
        {
            _logger.LogInformation("Delete [Users/" + id + "] ");

            Auth.ControllerHandler handler = (response) => Unauthorized(response);

            if (_auth.Authorize(handler, HttpContext))
            {
                try
                {
                    _authService.DeleteUserAccount(id);
                    return NoContent();
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
    }
}

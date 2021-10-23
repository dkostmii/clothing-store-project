using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Types;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace clothing_store_api.Middlewares
{
    public class Auth
    {
        private readonly TokenValidationParameters _params;

        public delegate ObjectResult ControllerHandler(object response);

        public Auth(TokenValidationParameters parameters)
        {
            _params = parameters;
        }

        public string GetToken(HttpContext ctx)
        {
            string? token = ctx.Request.Headers["Authorization"];

            if (token != null)
            {
                if (token.Split(" ")[0].ToLower() == "bearer")
                {
                    return token.Split(" ")[1];
                }
                throw new Exception("Provided token is not bearer");
            }

            return "";
        }

        public Role GetRole(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (String.IsNullOrEmpty(token))
            {
                throw new Exception("Token is not provided");
            }

            var claims = handler.ValidateToken(token, _params, out var tokenSecure).Claims;

            var roleClaim = claims.Where(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)
                .FirstOrDefault();
            
            if (roleClaim == null)
            {
                throw new Exception("Invalid token");
            }

            var strRole = roleClaim.Value;
            return Role.GetRole(strRole);
        }

        public bool ValidRole(Role role)
        {
            return Role.Customer.Value == role.Value ||
                Role.Manager.Value == role.Value;
        }

        public bool AuthorizeByRole(ControllerHandler handlerUnauth, HttpContext ctx, Role role)
        {
            try
            {
                var token = GetToken(ctx);

                if (String.IsNullOrEmpty(token))
                {
                    throw new Exception("No token provided.");
                }

                var tokenRole = GetRole(token);
                if (!ValidRole(tokenRole))
                {
                    throw new Exception("Invalid Role in token.");
                }

                if (role.Value != tokenRole.Value)
                {
                    throw new Exception("Role is unathorized.");
                }

            }
            catch (Exception ex)
            {
                handlerUnauth(new { Message = ex.Message });
                return false;
            }

            return true;
        }

        public bool Authorize(ControllerHandler handlerUnauth, HttpContext ctx)
        {
            try
            {
                var token = GetToken(ctx);

                if (String.IsNullOrEmpty(token))
                {
                    throw new Exception("No token provided.");
                }

                if (!ValidRole(GetRole(token)))
                {
                    throw new Exception("Invalid Role in token.");
                }
            }
            catch (Exception ex)
            {
                handlerUnauth(new { Message = ex.Message });
                return false;
            }

            return true;
        }
    }
}

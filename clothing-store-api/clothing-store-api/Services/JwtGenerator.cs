using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using clothing_store_api.Models;
using clothing_store_api.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace clothing_store_api.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;

        public JwtGenerator(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Jwt:Key")));
            _config = config;
        }

        public string CreateToken(UserRole data)
        {
            var claims = new List<Claim>
            { 
                new Claim(JwtRegisteredClaimNames.NameId, data.User.FirstName + " " + data.User.LastName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, data.Role.Value)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config.GetValue<string>("Jwt:Issuer"),
                Audience = _config.GetValue<string>("Jwt:Issuer"),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

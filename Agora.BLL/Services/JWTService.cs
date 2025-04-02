using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Agora.BLL.Services
{
    public class JWTService: IJWTService
    {
        private readonly IConfiguration _config;
        public JWTService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(UserDTO userDTO, RoleDTO roleDTO)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDTO.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, roleDTO.Id.ToString()), 
                new Claim(ClaimTypes.Role, roleDTO.Role), 
                new Claim(ClaimTypes.UserData, userDTO.Id.ToString()) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RoleDTO DecryptJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);


            var role = principal.FindFirst(ClaimTypes.Role)?.Value;
            var stringId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int id = 0;
            Int32.TryParse(stringId, out id);

            var stringUserId = principal.FindFirst(ClaimTypes.UserData)?.Value;
            int userId = 0;
            Int32.TryParse(stringUserId, out userId);

            var email = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            RoleDTO roleDTO = new RoleDTO();
            roleDTO.Role = role;
            roleDTO.Id = id;
            roleDTO.UserId = userId;
            return roleDTO;

        }


    }
}

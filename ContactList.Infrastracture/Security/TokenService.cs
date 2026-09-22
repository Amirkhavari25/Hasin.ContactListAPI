using ContactList.Application.Contracts.Security;
using ContactList.Application.DTOs;
using ContactList.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactList.Infrastracture.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<CustomResult<string>> CreateToken(User user)
        {
            try
            {
                var secretKey = _config["JwtSettings:SecretKey"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new ArgumentNullException(nameof(secretKey), "JWT Secret Key is missing in configuration.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
                    new Claim("username", user.Username),
                };

                var jwtToken = new JwtSecurityToken(
                    issuer: _config["JwtSettings:Issuer"],
                    audience: _config["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["JwtSettings:ExpiryMinutes"])),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.WriteToken(jwtToken);

                return await Task.FromResult(CustomResult<string>.SuccessResult(token));
            }
            catch (Exception ex)
            {
                //Log error
                return await Task.FromResult(CustomResult<string>.FailureResult("Something went wrong to create token"));
            }

        }
    }
}

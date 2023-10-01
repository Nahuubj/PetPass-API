using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PetPass_API.Models;
using PetPass_API.Models.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using PetPass_API.Data;

namespace PetPass_API.Services
{
    public class AuthService : IAuthService
    {

        private readonly DbPetPassContext _context;
        private readonly IConfiguration _config;

        public AuthService(DbPetPassContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponse> TokenReturnLogin(string username, string password)
        {
            var userFinded = _context.Users.FirstOrDefault(x =>
            x.Username == username && x.Userpassword == password);

            if (userFinded == null)
            {
                return await Task.FromResult<AuthResponse>(null);
            }

            string token = GenerateToken(userFinded.PersonId.ToString());

            bool valid = IsFirstLogin(username, password);

            return new AuthResponse() { Token = token, Result = valid, Msg="Ok" };
        }

        private string GenerateToken(string idUser)
        {
            var key = _config.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUser));

            var TokenCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = TokenCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string createdToken = tokenHandler.WriteToken(tokenConfig);

            return createdToken;
        }

        private bool IsFirstLogin(string username, string password)
        {
            bool validate;
            var user = _context.Users.FirstOrDefault(x =>
            x.Username == username && x.Userpassword == password);

            if (user.FirstSessionLogin == Char.ToString('1'))
                validate = true;
            else
                validate = false;

            return validate;
        }
    }
}

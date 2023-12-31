﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PetPass_API.Models;
using PetPass_API.Models.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using PetPass_API.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<AuthResponse> TokenReturnLogin(UserRequest userRequest)
        {
            var userFinded = _context.Users.FirstOrDefault(x =>
            x.Username == userRequest.Username && x.Userpassword == userRequest.Userpassword);

            if (userFinded == null)
            {
                return await Task.FromResult<AuthResponse>(null);
            }

            string photo = null; // Inicializa 'photo' como null por defecto

            if (userFinded.Rol == "B")
            {
                // Obtén la ruta de la imagen desde la base de datos
                photo = await _context.ConfigUsers
                    .Where(x => x.PersonId == userFinded.PersonId)
                    .Select(x => x.PathImages)
                    .FirstOrDefaultAsync();
            }

            string token = GenerateToken(userFinded.PersonId.ToString());

            bool valid = IsFirstLogin(userFinded.Username, userFinded.Userpassword);

            return new AuthResponse() { userID = userFinded.PersonId ,Token = token, FirstLogin = valid, Role = Char.Parse(userFinded.Rol.ToString()), Photo = photo};
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

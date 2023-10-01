using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;
using PetPass_API.Models.Custom;
using PetPass_API.Services;
using System.Net.Mail;

namespace PetPass_API.Controllers
{
    [Route("PetPass/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbPetPassContext _context;
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService, DbPetPassContext context)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var resultAuth = await _authService.TokenReturnLogin(username, password);
            if(resultAuth == null)
                return Unauthorized();

            return Ok(resultAuth);
        }

        //login(manejo de sesiones), cambiar contraseña, primer inicio sesion
        [Authorize]
        [HttpPut]
        [Route("firstPassword")]
        public async Task<IActionResult> firstPassword(int userID, string newPassword)
        {
            if (userID == null || _context.People == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.PersonId == userID);


            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Userpassword = newPassword;
                user.FirstSessionLogin = Char.ToString(('0'));
                _context.Entry(user).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut]
        [Route("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPassword(int userID, string codeRecovery, string newPassword)
        {
            if (userID == null || _context.People == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.PersonId == userID && m.CodeRecovery == codeRecovery);


            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Userpassword = newPassword;
                _context.Entry(user).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }


        [HttpPut]
        [Route("FindByEmail")]
        public async Task<IActionResult> FindByEmail(string? email)
        {
            if (email == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.Email == email);

            if (person == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.PersonId == person.PersonId);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var code = GenerateCodeRecovery();

                user.CodeRecovery = code;
                _context.Entry(user).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            SendEmail(email, user.CodeRecovery);

            return Ok(user.PersonId);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GenerateCodeRecovery()
        {
            var code = Guid.NewGuid().ToString().Split("-")[0];

            return code;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void SendEmail(string EmailDestiny, string codeRecovery)
        {
            string EmailOrigin = "nahuel.gutierrez.vargas17@gmail.com";
            string password = "bdzqnmwcnemhbqub\r\n";

            MailMessage mailMessage = new MailMessage(EmailOrigin, EmailDestiny, "Solicitud de Cambio de Contraseña",
                "<p>Este es tu codigo de recuperacion: </p><br />" +
                codeRecovery);

            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigin, password);
            smtpClient.Send(mailMessage);
            smtpClient.Dispose();
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;
using PetPass_API.Models.Custom;
using PetPass_API.Services;
using System.Net.Mail;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

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
        // A PARTIR DEL 29 10 2023 DEBE ENVIAR CONTRASEÑA CIFRADA
        public async Task<IActionResult> Login([FromBody] UserRequest request)
        {

            var resultAuth = await _authService.TokenReturnLogin(request);
            if(resultAuth == null)
                return Unauthorized();

            return Ok(resultAuth);
        }

        /*Necesitas crearte un modelo desde la aplicacion para poder enviar,
          DEBE SER CON CONTRASEÑA YA CIFRADA A PARTIR DEL 29 10 2023 */
        [Authorize]
        [HttpPut]
        [Route("firstPassword")]
        public async Task<IActionResult> firstPassword([FromBody] FirstLoginUser fUser)
        {
            if (fUser == null || _context.People == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.PersonId == fUser.userID);


            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Userpassword = fUser.newPassword;
                user.FirstSessionLogin = Char.ToString('0');
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

        #region DIEGO RICALDEZ METHODS
        [HttpPut]
        [Route("RecoveryPassword")]
        /* NECESITAS CREARTE UN MODELO QUE ENVIE LOS PARAMETROS HABLADOS ANTERIORMENTE,
           SI NECESITAS VER CUALES SON CLICK DERECHO IR A DEFINICION CIFRADOS CONTRASEÑA Y CODIGO*/
        public async Task<IActionResult> RecoveryPassword([FromBody] AuthRecoveryPassword recoveryPassword)
        {
            if (recoveryPassword == null || _context.People == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.PersonId == recoveryPassword.UserID && m.CodeRecovery == recoveryPassword.CodeRecovery);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.Userpassword = recoveryPassword.newPassword;
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
            var code = "";
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
                
                code = GenerateCodeRecovery();

                user.CodeRecovery = GetSha256(code);
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
            SendEmail(email, code);

            return Ok(user.PersonId);
        }

        #endregion

        #region Code Recovery Methods
        [ApiExplorerSettings(IgnoreApi = true)]
        private string GenerateCodeRecovery()
        {
            var code = Guid.NewGuid().ToString().Split("-")[0];

            return code;
        }

        private string GetSha256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        private void SendEmail(string EmailDestiny, string codeRecovery)
        {
            string EmailOrigin = "nahuel.gutierrez.vargas17@gmail.com";
            string password = "pbek lzxr uxvd byux\r\n";

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
        #endregion
    }

}


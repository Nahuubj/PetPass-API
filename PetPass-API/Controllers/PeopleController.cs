using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;
using PetPass_API.Models.Custom;

namespace PetPass_API.Controllers
{
    [Route("PetPass/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly DbPetPassContext _context;

        public PeopleController(DbPetPassContext context)

        {
            _context = context;

        }

        // GET: People
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.People.ToListAsync());
        }

        // GET: People/Details/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

    // POST: api/People
        [Authorize]
        [HttpPost]
        [Route("CreateBrigadier")]
        public async Task<ActionResult<Person>> CreateBrigadier(Person person)
        {
            if (person != null)
            {
                User user = new User();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.People.AddAsync(person);
                        await _context.SaveChangesAsync();

                        user.PersonId = person.PersonId;
                        user.Username = GenerateUserName(person.Name, person.FirstName, person.Email);
                        user.Userpassword = GeneratePassword();
                        user.Rol = "B";
                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        SendEmail(person.Email, user.Username, user.Userpassword);
                        return CreatedAtAction("Details", new { id = person.PersonId }, person);
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            return BadRequest();
        }

        //public async Task<IActionResult> UploadFile(UploadImage brigadierFile)
        //{

        //}

        [Authorize]
        [HttpPost]
        [Route("CreateOwner")]
        public async Task<ActionResult<Person>> CreateOwner(Person person)
        {
            if (person != null)
            {
                User user = new User();
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.People.AddAsync(person);
                        await _context.SaveChangesAsync();

                        user.PersonId = person.PersonId;
                        user.Username = GenerateUserName(person.Name, person.FirstName, person.Email);
                        user.Userpassword = GeneratePassword();
                        user.Rol = "O";
                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();
                        //MANEJAR SESIONES, PARA RELLENAR PERSON_REGISTER
                        await transaction.CommitAsync();
                        SendEmail(person.Email, user.Username, user.Userpassword);
                        return CreatedAtAction("Details", new { id = person.PersonId }, person);
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            return BadRequest();
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut]
        [Route("UpdatePerson")]
        public async Task<IActionResult> PutPerson(Person person)
        {
            if (!PersonExists(person.PersonId))
            {
                return NotFound();
            }
            else
            {
                _context.Entry(person).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: People/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [Route("DeletePerson")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'DbpetPassContext.People'  is null.");
            }
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                person.State = 0;
                _context.Entry(person).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.PersonId == id);
        }

        #region Create User

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GenerateUserName(string name, string lastName, string email)
        {
            string characters = "0123456789";
            string newName = name.Substring(0, 1).ToLower();
            string newLastName = lastName.Substring(0, 1).ToLower();
            string newEmail = email.Substring(0, 5).ToLower();
            string newCharacters = "";
            Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                int s = random.Next(0, characters.Length);
                newCharacters = newCharacters + characters[s];
            }

            string UserName = string.Concat(newName, newLastName, newEmail, newCharacters);
            return UserName;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GeneratePassword()
        {
            string characters = @"ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789_-.@*$";
            string password = "";
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                int s = random.Next(0, characters.Length);
                password = password + characters[s];
            }
            return password;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void SendEmail(string EmailDestiny, string userName, string userPassword)
        {
            string EmailOrigin = "nahuel.gutierrez.vargas17@gmail.com";
            string password = "pbek lzxr uxvd byux\r\n";

            MailMessage mailMessage = new MailMessage(EmailOrigin, EmailDestiny, "Bienvenido a PetPass!",
                "<p>Nombre de Usuario: </p><br />" + 
                userName + 
                "<p>Contraseña: </p><br />" +
                userPassword);

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


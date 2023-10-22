using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;
using PetPass_API.Services;
using QRCoder;
using System;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;

namespace PetPass_API.Controllers
{

    [Route("PetPass/[controller]")]
    [ApiController]    
    public class PetsController : ControllerBase
    {
        private readonly DbPetPassContext _context;

        public PetsController(DbPetPassContext context) 
        {
            _context = context;
        }

        // GET: Pets
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Pets.ToListAsync());
        }

        // GET: Pets/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(pet);
        }

        [HttpPost]
        [Route("CreatePet")]
        public async Task<ActionResult<Pet>> CreatePet(Pet pet, int userId)
        {
            if (pet != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var person = await _context.People
                            .FirstOrDefaultAsync(p => p.PersonId == pet.PersonId);
                        if (person == null)
                        {
                            return NotFound();
                        }

                        await _context.Pets.AddAsync(pet);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        PetRegisterService petRegister = new PetRegisterService(_context);
                        petRegister.RegisterPet(pet.PetId, userId);

                        QRCodeService qRCodeService = new QRCodeService();
                        qRCodeService.GenerateAndSendQRCode(pet.PetId, person.Email);
                        return CreatedAtAction("Details", new { id = pet.PetId }, pet);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPet(Pet pet)
        {
            if (!PetExists(pet.PetId))
            {
                return NotFound();
            }
            else
            {
                _context.Entry(pet).State = EntityState.Modified;
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

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("DeletePet")]
        public async Task<IActionResult> DeletePet(int id)
        {
            if (_context.Pets == null)
            {
                return Problem("Entity set 'DbpetPassContext.People'  is null.");
            }
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                pet.State = 0;
                _context.Entry(pet).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        [Route("GetPetQR")]
        public async Task<IActionResult> GetPetQR(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == id);
            if (pet == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(p => p.PersonId == pet.PersonId);

            if (person == null)
            {
                return NotFound();
            }

            string OwnerName = person.Name + " " + person.FirstName;

            DTOPet dtoPet = new DTOPet
            {
                petId = pet.PetId,
                ownerName = OwnerName,
                ci = person.Ci,
                petName = pet.Name,
                specie = pet.Specie,
                breed = pet.Breed,
                gender = pet.Gender,
                description = pet.SpecialFeature
            };
            return Ok(dtoPet);
        }



        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetId == id);
        }


        [HttpGet]
        [Route("FindByCI")]
        public async Task<IActionResult> FindByCI(string? ci)
        {

            if (ci == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.Ci == ci);

            return Ok(person);
        }
    }
}

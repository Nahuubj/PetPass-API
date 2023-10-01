using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;
using System;
using System.Drawing;

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
        public async Task<ActionResult<Pet>> CreatePet(Pet pet)
        {
            if (pet != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.Pets.AddAsync(pet);
                        await _context.SaveChangesAsync();
                        //MANEJAR SESIONES, PARA RELLENAR PERSON_REGISTER
                        await transaction.CommitAsync();
                        return CreatedAtAction("Details", new { id = pet.PetId }, pet);

                        //FALTA IMPLEMENTAR QR
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

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetId == id);
        }
    }
}

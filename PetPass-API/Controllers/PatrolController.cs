using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;
using PetPass_API.Services;
using System;

namespace PetPass_API.Controllers
{
    [Route("PetPass/[controller]")]
    [ApiController]
    public class PatrolController : Controller
    {
        private readonly DbPetPassContext _context;

        public PatrolController(DbPetPassContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Patrols.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patrols == null)
            {
                return NotFound();
            }

            var patrol = await _context.Patrols
                .FirstOrDefaultAsync(p => p.PatrolId == id);

            if (patrol == null)
            {
                return NotFound();
            }
            return Ok(patrol);
        }

        [HttpPost]
        [Route("CreatePatrol")]
        public async Task<ActionResult<Pet>> CreatePet([FromBody]Patrol patrol)
        {
            if (patrol != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await _context.Patrols.AddAsync(patrol);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return Ok();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetZones")]
        public async Task<IActionResult> GetZones()
        {
            return Ok(await _context.Zones.ToListAsync());
        }
    }
}

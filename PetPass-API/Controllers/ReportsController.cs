using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;

namespace PetPass_API.Controllers
{
    [Route("PetPass/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        private readonly DbPetPassContext _context;

        public ReportsController(DbPetPassContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("GetPetsRegisteredByBrigadiers")]
        public async Task<IActionResult> GetPetsRegisteredByBrigadiers()
        {
            var brigadiers = await _context.Users
                .Where(u => u.Rol == "B" && _context.PetRegisters.Any(pr => pr.UserPersonId == u.PersonId))
                .ToListAsync();

            var brigadierData = new List<object>();

            foreach (var brigadier in brigadiers)
            {
                var brigadierPerson = await _context.People
                    .FirstOrDefaultAsync(p => p.PersonId == brigadier.PersonId);

                var totalPets = await _context.PetRegisters
                    .Where(pr => pr.UserPersonId == brigadier.PersonId)
                    .CountAsync();

                if (totalPets > 0)
                {
                    brigadierData.Add(new
                    {
                        BrigadierName = $"{brigadierPerson.Name} {brigadierPerson.FirstName}",
                        TotalPetsRegistered = totalPets
                    });
                }
            }
            return Ok(brigadierData);
        }

        [Authorize]
        [HttpGet]
        [Route("GetZoneReport")]
        public async Task<IActionResult> GetZoneReport()
        {
            try
            {
                var zoneReport = await _context.Zones
                    .Select(zone => new
                    {
                        ZoneName = zone.Name,
                        TotalBrigadiers = _context.Patrols
                            .Where(patrol => patrol.ZoneId == zone.ZoneId)
                            .Select(patrol => patrol.PersonId)
                            .Distinct()
                            .Count()
                    })
                    .Where(report => report.TotalBrigadiers > 0)
                    .ToListAsync();

                return Ok(zoneReport);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error en la solicitud: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetZoneAndTotalDogs")]
        public async Task<IActionResult> GetZoneAndTotalDogs()
        {
            try
            {
                var zoneReport = await _context.Zones
                    .Select(zone => new
                    {
                        ZoneName = zone.Name,
                        TotalDogs = _context.Patrols
                            .Where(patrol => patrol.ZoneId == zone.ZoneId)
                            .SelectMany(patrol => patrol.Person.PetRegisters)
                            .Count()
                    })
                    .Where(report => report.TotalDogs > 0)
                    .ToListAsync();

                return Ok(zoneReport);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error en la solicitud: {ex.Message}");
            }
        }
    }
}

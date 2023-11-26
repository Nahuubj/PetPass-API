using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;
using PetPass_API.Services;

namespace PetPass_API.Controllers
{
    [Route("PetPass/[controller]")]
    [ApiController]
    public class CampaignsController : Controller
    {
        private readonly DbPetPassContext _context;

        public CampaignsController(DbPetPassContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        // GET: Campaigns
        public async Task<IActionResult> Index()
        {
              return _context.Campaigns != null ? 
                          Ok(await _context.Campaigns.ToListAsync()) :
                          Problem("Entity set 'DbpetPassContext.Campaigns'  is null.");
        }

        [Authorize]
        [HttpGet("{id}")]
        // GET: Campaigns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Campaigns == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .FirstOrDefaultAsync(m => m.CampaignId == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return Ok(campaign);
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Campaign campaign)
        {
            try
            {

                await _context.Campaigns.AddAsync(campaign);
                await _context.SaveChangesAsync();

                return CreatedAtAction("Details", new { id = campaign.CampaignId }, campaign);

            }
            catch
            {
                return BadRequest();
            }
        }

    }
}

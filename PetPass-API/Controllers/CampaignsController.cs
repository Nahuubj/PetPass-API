using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;

namespace PetPass_API.Controllers
{
    public class CampaignsController : Controller
    {
        private readonly DbPetPassContext _context;

        public CampaignsController(DbPetPassContext context)
        {
            _context = context;
        }

        // GET: Campaigns
        public async Task<IActionResult> Index()
        {
              return _context.Campaigns != null ? 
                          View(await _context.Campaigns.ToListAsync()) :
                          Problem("Entity set 'DbpetPassContext.Campaigns'  is null.");
        }

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

            return View(campaign);
        }

        // GET: Campaigns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Campaigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CampaignId,Name,CampaignDate")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                _context.Add(campaign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(campaign);
        }

        // GET: Campaigns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Campaigns == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }
            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CampaignId,Name,CampaignDate")] Campaign campaign)
        {
            if (id != campaign.CampaignId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campaign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaignExists(campaign.CampaignId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(campaign);
        }

        // GET: Campaigns/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(campaign);
        }

        // POST: Campaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Campaigns == null)
            {
                return Problem("Entity set 'DbpetPassContext.Campaigns'  is null.");
            }
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign != null)
            {
                _context.Campaigns.Remove(campaign);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampaignExists(int id)
        {
          return (_context.Campaigns?.Any(e => e.CampaignId == id)).GetValueOrDefault();
        }
    }
}

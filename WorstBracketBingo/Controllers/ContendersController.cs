using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorstBracketBingo.Data;
using WorstBracketBingo.Models;

namespace WorstBracketBingo.Controllers
{
    public class ContendersController : Controller
    {
        private readonly BingoContext _context;

        public ContendersController(BingoContext context)
        {
            _context = context;    
        }

        // GET: Contenders
        public async Task<IActionResult> Index()
        {
            var bingoContext = _context.Contenders.Include(c => c.Entrant);
            return View(await bingoContext.ToListAsync());
        }

        // GET: Contenders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contender = await _context.Contenders
                .Include(c => c.Entrant)
                .SingleOrDefaultAsync(m => m.ContenderID == id);
            if (contender == null)
            {
                return NotFound();
            }

            return View(contender);
        }

        // GET: Contenders/Create
        public IActionResult Create()
        {
            ViewData["EntrantID"] = new SelectList(_context.Entrants, "EntrantID", "Name");
            return View();
        }

        // POST: Contenders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContenderID,EntrantID,BracketID,Eliminated,RoundsAlive")] Contender contender)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contender);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["EntrantID"] = new SelectList(_context.Entrants, "EntrantID", "Name", contender.EntrantID);
            return View(contender);
        }

        // GET: Contenders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contender = await _context.Contenders.SingleOrDefaultAsync(m => m.ContenderID == id);
            if (contender == null)
            {
                return NotFound();
            }
            ViewData["EntrantID"] = new SelectList(_context.Entrants, "EntrantID", "Name", contender.EntrantID);
            return View(contender);
        }

        // POST: Contenders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContenderID,EntrantID,BracketID,Eliminated,RoundsAlive")] Contender contender)
        {
            if (id != contender.ContenderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contender);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContenderExists(contender.ContenderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["EntrantID"] = new SelectList(_context.Entrants, "EntrantID", "Name", contender.EntrantID);
            return View(contender);
        }

        // GET: Contenders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contender = await _context.Contenders
                .Include(c => c.Entrant)
                .SingleOrDefaultAsync(m => m.ContenderID == id);
            if (contender == null)
            {
                return NotFound();
            }

            return View(contender);
        }

        // POST: Contenders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contender = await _context.Contenders.SingleOrDefaultAsync(m => m.ContenderID == id);
            _context.Contenders.Remove(contender);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ContenderExists(int id)
        {
            return _context.Contenders.Any(e => e.ContenderID == id);
        }
    }
}

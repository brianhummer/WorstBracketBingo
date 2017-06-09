using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorstBracketBingo.Data;
using WorstBracketBingo.Models;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;

namespace WorstBracketBingo.Controllers
{
    public class EntrantsController : Controller
    {
        private readonly BingoContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EntrantsController(BingoContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;    
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Entrants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Entrants.ToListAsync());
        }

        // GET: Entrants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrant = await _context.Entrants
                .SingleOrDefaultAsync(m => m.EntrantID == id);
            if (entrant == null)
            {
                return NotFound();
            }

            return View(entrant);
        }

        // GET: Entrants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entrants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntrantID,Name,Tag")] Entrant entrant)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {

                        var file = Image;
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "images\\uploads");
                        
                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.Trim('"');
                            var fileType = fileName.Split('.').Last();
                            var uniqueFileName = string.Format(@"{0}." + fileType, DateTime.Now.Ticks);

                            using (var fileStream = new FileStream(Path.Combine(uploads, uniqueFileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                entrant.Thumbnail = uniqueFileName;
                            }


                        }
                    }
                }

                _context.Add(entrant);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(entrant);
        }

        // GET: Entrants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrant = await _context.Entrants.SingleOrDefaultAsync(m => m.EntrantID == id);
            if (entrant == null)
            {
                return NotFound();
            }
            return View(entrant);
        }

        // POST: Entrants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EntrantID,Name,Tag,Thumbnail")] Entrant entrant)
        {
            if (id != entrant.EntrantID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntrantExists(entrant.EntrantID))
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
            return View(entrant);
        }

        // GET: Entrants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrant = await _context.Entrants
                .SingleOrDefaultAsync(m => m.EntrantID == id);
            if (entrant == null)
            {
                return NotFound();
            }

            return View(entrant);
        }

        // POST: Entrants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entrant = await _context.Entrants.SingleOrDefaultAsync(m => m.EntrantID == id);

            var path = Path.Combine(_hostingEnvironment.WebRootPath, "images\\uploads", entrant.Thumbnail);
            if ((System.IO.File.Exists(path)))
            {
                System.IO.File.Delete(path);
            }

            _context.Entrants.Remove(entrant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EntrantExists(int id)
        {
            return _context.Entrants.Any(e => e.EntrantID == id);
        }
    }
}

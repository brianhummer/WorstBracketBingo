using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorstBracketBingo.Data;
using WorstBracketBingo.Models;
using WorstBracketBingo.Models.BracketBingoViewModels;

namespace WorstBracketBingo.Controllers
{
    public class BingoBoardsController : Controller
    {
        private readonly BingoContext _context;

        public BingoBoardsController(BingoContext context)
        {
            _context = context;    
        }

        // GET: BingoBoards
        public async Task<IActionResult> Index()
        {
            return View(await _context.BingoBoards.ToListAsync());
        }

        // GET: BingoBoards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bingoBoard = await _context.BingoBoards
                .SingleOrDefaultAsync(m => m.BingoBoardID == id);
            if (bingoBoard == null)
            {
                return NotFound();
            }

            return View(bingoBoard);
        }

        // GET: BingoBoards/Create
        public IActionResult Create()
        {
            var board = new BingoBoard();
            board.BoardPieces = new List<BoardPiece>();
            return View();
        }

        // POST: BingoBoards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BingoBoardID,BracketID,Title")] BingoBoard bingoBoard, string[] selectedContenders)
        {
            if (selectedContenders != null)
            {
                List<int> positions = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
                var rand = new Random();

                bingoBoard.BoardPieces = new List<BoardPiece>();
                foreach (var contender in selectedContenders)
                {
                    var index = rand.Next(0, positions.Count);
                    positions.Remove(positions[index]);
                    
                    var contenderToAdd = new BoardPiece { BingoBoardID = bingoBoard.BingoBoardID, ContenderID = int.Parse(contender), BoardPosition = index };
                    bingoBoard.BoardPieces.Add(contenderToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(bingoBoard);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bingoBoard);
        }

        // GET: BingoBoards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bingoBoard = await _context.BingoBoards
                .Include(b => b.BoardPieces)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BingoBoardID == id);
            if (bingoBoard == null)
            {
                return NotFound();
            }
            return View(bingoBoard);
        }

        // POST: BingoBoards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BingoBoardID,BracketID,Title")] BingoBoard bingoBoard)
        {
            if (id != bingoBoard.BingoBoardID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bingoBoard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BingoBoardExists(bingoBoard.BingoBoardID))
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
            return View(bingoBoard);
        }

        // GET: BingoBoards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bingoBoard = await _context.BingoBoards
                .SingleOrDefaultAsync(m => m.BingoBoardID == id);
            if (bingoBoard == null)
            {
                return NotFound();
            }

            return View(bingoBoard);
        }

        // POST: BingoBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bingoBoard = await _context.BingoBoards.SingleOrDefaultAsync(m => m.BingoBoardID == id);
            _context.BingoBoards.Remove(bingoBoard);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BingoBoardExists(int id)
        {
            return _context.BingoBoards.Any(e => e.BingoBoardID == id);
        }
    }
}

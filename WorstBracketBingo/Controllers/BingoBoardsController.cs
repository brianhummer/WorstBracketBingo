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
using WorstBracketBingo.Models.BingoBoardViewModels;

namespace WorstBracketBingo.Controllers
{
    public class BingoBoardsController : Controller
    {
        private const int REQUIRED_PIECES = 25;
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
                .Include(b => b.Bracket)
                .Include(b => b.BoardPieces)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BingoBoardID == id);
            if (bingoBoard == null)
            {
                return NotFound();
            }

            return View(bingoBoard);
        }

        // GET: BingoBoards/Create
        public IActionResult Create(int id)
        {
            var bingoBoardViewModel = new BoardCreateViewModel();
            bingoBoardViewModel.BracketID = id;
            ViewData["BracketTitle"] = _context.Brackets.Single(b => b.BracketID == id).Title;

            //bingoBoardViewModel.Entrants = new List<Entrant>(25);
            //bingoBoard.BoardPieces = new List<BoardPiece>();
            //bingoBoard.RoundEliminated = -1;

            ViewBag.Entrants = new SelectList(_context.Entrants.OrderBy(e => e.Name).AsNoTracking(), "EntrantID", "Name");

            return View(bingoBoardViewModel);
        }

        // POST: BingoBoards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BoardCreateViewModel bingoBoardViewModel)
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < REQUIRED_PIECES; i++)
                positions.Add(i);

            // TODO: validate distinct
            //var distinctEntrants = bingoBoard.Entrants.Select(e => e.EntrantID).Distinct().ToList();
            
            var rand = new Random(DateTime.Now.Millisecond);
            var bracket = _context.Brackets
                .Include(b => b.Contenders)
                .Include(b => b.Rounds)
                .Single(b => b.BracketID == bingoBoardViewModel.BracketID);

            var bracketEntrants = bracket.Contenders.Select(b => b.EntrantID).ToList();
            var contenders = new List<Contender>();
            var roundId = bracket.Rounds != null && bracket.Rounds.Count == 1 ? bracket.Rounds.First().RoundID : -1;
            for (int i = 0; i < bingoBoardViewModel.Entrants.Count; i++)
            {
                Contender contender;
                if(!bracketEntrants.Contains(bingoBoardViewModel.Entrants[i]))
                {
                    contender = new Contender
                    {
                        BracketID = bracket.BracketID,
                        EntrantID = bingoBoardViewModel.Entrants[i],
                    };

                     _context.Add(contender);

                    if(roundId != -1)
                    {
                        _context.Add(new RoundContender { RoundID = roundId, ContenderID = contender.ContenderID });
                    }
                }
                else
                {
                    contender = bracket.Contenders.Single(c => c.EntrantID == bingoBoardViewModel.Entrants[i]);
                }

                contenders.Add(contender);
            }

            //await _context.SaveChangesAsync();

            var bingoBoard = new BingoBoard
            {
                BracketID = bracket.BracketID,
                Title = bingoBoardViewModel.Title,
                RoundEliminated = -1
            };

            bingoBoard.BoardPieces = new List<BoardPiece>();
            for (int i = 0; i < contenders.Count; i++)
            {
                var pos = i == 0 ? positions.Count / 2 : rand.Next(0, positions.Count);
                var boardPieceToAdd = new BoardPiece { BingoBoardID = bingoBoard.BingoBoardID, ContenderID = contenders[i].ContenderID, BoardPosition = positions[pos]};
                positions.Remove(positions[pos]);

                bingoBoard.BoardPieces.Add(boardPieceToAdd);
            }

            if (ModelState.IsValid)
            {
                _context.Add(bingoBoard);
                await _context.SaveChangesAsync();
                return RedirectToAction("Manage", "Brackets", new { id = bingoBoard.BracketID });
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
                .Include(b => b.BoardPieces).ThenInclude(b => b.Contender)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BingoBoardID == id);
            if (bingoBoard == null)
            {
                return NotFound();
            }
            PopulateAssignedPieceData(bingoBoard);
            return View(bingoBoard);
        }

        // POST: BingoBoards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedContenders)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardToUpdate = await _context.BingoBoards
                .Include(i => i.BoardPieces)
                    .ThenInclude(i => i.Contender)
                .SingleOrDefaultAsync(m => m.BingoBoardID == id);

            if (await TryUpdateModelAsync<BingoBoard>(boardToUpdate, "", i => i.Title))
            {
                UpdateBoardContenders(selectedContenders, boardToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction("Index");
            }
            UpdateBoardContenders(selectedContenders, boardToUpdate);
            PopulateAssignedPieceData(boardToUpdate);
            return View(boardToUpdate);
        }

        private void PopulateBracketsDropDownList(object selectedBracket = null)
        {
            var bracketQuery = from b in _context.Brackets
                                   orderby b.Title
                                   select b;
            ViewBag.BracketID = new SelectList(bracketQuery.AsNoTracking(), "BracketID", "Title", selectedBracket);
        }

        private void PopulateAssignedPieceData(BingoBoard bingoBoard)
        {
            var allContenders = _context.Contenders
                .Where(b => b.BracketID == bingoBoard.BracketID)
                .Include(c => c.Entrant);
            var bracketContenders = new HashSet<int>(bingoBoard.BoardPieces.Select(c => c.ContenderID));
            var viewModel = new List<AssignedContenderData>();
            foreach (var contender in allContenders)
            {
                viewModel.Add(new AssignedContenderData
                {
                    ContenderID = contender.ContenderID,
                    Name = contender.Entrant.Name,
                    Thumbnail = contender.Entrant.Thumbnail,
                    Assigned = bracketContenders.Contains(contender.ContenderID)
                });
            }
            ViewData["Contenders"] = viewModel;
        }

        private void UpdateBoardContenders(string[] selectedContenders, BingoBoard boardToUpdate)
        {
            if (selectedContenders == null)
            {
                boardToUpdate.BoardPieces = new List<BoardPiece>();
                return;
            }

            List<int> positions = new List<int>();
            for (int i = 1; i <= REQUIRED_PIECES; i++)
                positions.Add(i);
            var rand = new Random(DateTime.Now.Millisecond);

            var selectedContendersHS = new HashSet<string>(selectedContenders);
            var boardContenders = new HashSet<int>(boardToUpdate.BoardPieces.Select(c => c.Contender.ContenderID));
            foreach (var contender in _context.Contenders)
            {
                if (selectedContendersHS.Contains(contender.ContenderID.ToString()))
                {
                    if (!boardContenders.Contains(contender.ContenderID))
                    {
                        boardToUpdate.BoardPieces.Add(new BoardPiece { BingoBoardID = boardToUpdate.BingoBoardID, ContenderID = contender.ContenderID});
                    }
                }
                else
                {
                    if (boardContenders.Contains(contender.EntrantID))
                    {
                        BoardPiece boardPieceToRemove = boardToUpdate.BoardPieces.SingleOrDefault(i => i.ContenderID == contender.ContenderID);
                        _context.Remove(boardPieceToRemove);
                    }
                }
            }

            foreach(var boardPiece in boardToUpdate.BoardPieces)
            {
                if (boardPiece.BoardPosition == 0)
                    positions.Remove(boardPiece.BoardPosition);
            }

            foreach(var boardPiece in boardToUpdate.BoardPieces)
            {
                if(boardPiece.BoardPosition == 0)
                {
                    var pos = rand.Next(0, positions.Count);
                    boardPiece.BoardPosition = positions[pos];
                    positions.Remove(positions[pos]);
                }
            }
        }

        // GET: BingoBoards/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

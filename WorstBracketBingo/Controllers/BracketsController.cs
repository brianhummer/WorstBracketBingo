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
    public class BracketsController : Controller
    {
        private readonly BingoContext _context;

        public BracketsController(BingoContext context)
        {
            _context = context;    
        }

        // GET: Brackets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brackets.ToListAsync());
        }

        // GET: Brackets/Details/5
        public async Task<IActionResult> Details(int? id, int? round)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .Include(b => b.Rounds)
                    .ThenInclude(r => r.RoundContenders)
                    .ThenInclude(r => r.Contender)
                    .ThenInclude(r => r.Entrant)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BracketID == id);

            if (bracket == null)
            {
                return NotFound();
            }

            int selectedRound = round == null ? bracket.Rounds.Count - 1 : (int)round;

            var bracketViewModel = new BracketDetailsViewModel();
            bracketViewModel.BracketID = bracket.BracketID;
            bracketViewModel.Title = bracket.Title;
            bracketViewModel.NumRounds = bracket.Rounds.Count;
            bracketViewModel.Round = bracket.Rounds.SingleOrDefault(r => r.RoundNumber == selectedRound);

            if(bracketViewModel.Round != null)
                bracketViewModel.Round.RoundContenders = bracketViewModel.Round.RoundContenders.OrderBy(r => r.Contender.Entrant.Name).ToList();

            return View(bracketViewModel);
        }

        // GET: Brackets/Create
        public IActionResult Create()
        {
            var bracket = new Bracket();
            bracket.Contenders = new List<Contender>();
            bracket.Rounds = new List<Round>();
            PopulateAssignedContenderData(bracket);
            //PopulateAssignedRoundData(bracket);
            return View();
        }

        // POST: Brackets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BracketID,Title")] Bracket bracket, string[] selectedEntrants)
        {
            if (selectedEntrants != null)
            {
                bracket.Contenders = new List<Contender>();
                foreach (var entrant in selectedEntrants)
                {
                    var entrantToAdd = new Contender { BracketID = bracket.BracketID, EntrantID = int.Parse(entrant), Eliminated = false, RoundsAlive = 0 };
                    bracket.Contenders.Add(entrantToAdd);
                }
            }

            bracket.Rounds = new List<Round>();
            

            if (ModelState.IsValid)
            {
                _context.Add(bracket);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            PopulateAssignedContenderData(bracket);
            return View(bracket);
        }

        // GET: Brackets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .Include(b => b.Contenders).ThenInclude(b => b.Entrant)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BracketID == id);
            if (bracket == null)
            {
                return NotFound();
            }
            PopulateAssignedContenderData(bracket);
            return View(bracket);
        }

        // POST: Brackets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedEntrants)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracketToUpdate = await _context.Brackets
                .Include(i => i.Contenders)
                    .ThenInclude(i => i.Entrant)
                .SingleOrDefaultAsync(m => m.BracketID == id);

            if (await TryUpdateModelAsync<Bracket>(bracketToUpdate, "", i => i.Title))
            {
                UpdateBracketEntrants(selectedEntrants, bracketToUpdate);
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
            UpdateBracketEntrants(selectedEntrants, bracketToUpdate);
            PopulateAssignedContenderData(bracketToUpdate);
            return View(bracketToUpdate);
        }

        // GET: Brackets/NextRound/5
        public async Task<IActionResult> NextRound(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracketToUpdate = await _context.Brackets
                .Include(i => i.Contenders)
                .Include(i => i.Rounds)
                .SingleOrDefaultAsync(m => m.BracketID == id);

            if (await TryUpdateModelAsync<Bracket>(bracketToUpdate, "", i => i.Title))
            {
                AddNewRound(bracketToUpdate);
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
                return RedirectToAction("Details",new { id = id });
            }
            AddNewRound(bracketToUpdate);
            return View(bracketToUpdate);
        }

        // GET: Brackets/ViewBoards/5
        public async Task<IActionResult> ViewBoards(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .Include(b => b.BingoBoards)
                .ThenInclude(b => b.BoardPieces)
                .ThenInclude(b => b.Contender)
                .ThenInclude(b => b.Entrant)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BracketID == id);

            if (bracket == null)
            {
                return NotFound();
            }

            foreach (var board in bracket.BingoBoards)
            {
                board.BoardPieces = board.BoardPieces.OrderBy(b => b.BoardPosition).ToList();
            }

            return View(bracket);
        }

        private void AddNewRound(Bracket bracketToUpdate)
        {
            var round = new Round();
            round.RoundNumber = bracketToUpdate.Rounds.Count;
            round.RoundContenders = new List<RoundContender>();

            foreach (var contender in bracketToUpdate.Contenders)
            {
                if (!contender.Eliminated)
                {
                    round.RoundContenders.Add(new RoundContender { RoundID = round.RoundID, ContenderID = contender.ContenderID });
                    contender.RoundsAlive++;
                }
            }

            bracketToUpdate.Rounds.Add(round);
        }

        private void PopulateAssignedRoundData(Round round)
        {
            var allContenders = _context.Contenders;
            var roundContenders = new HashSet<int>(round.RoundContenders.Select(c => c.ContenderID));
            var viewModel = new List<AssignedRoundData>();
            foreach (var contender in allContenders)
            {
                viewModel.Add(new AssignedRoundData
                {
                    //ContenderID = contender.ContenderID,
                   // Title = course.Title,
                   // Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewData["Round"] = viewModel;
        }


        private void PopulateAssignedContenderData(Bracket bracket)
        {
            var allEntrants = _context.Entrants;
            var bracketEntrants = new HashSet<int>(bracket.Contenders.Select(c => c.EntrantID));
            var viewModel = new List<AssignedEntrantData>();
            foreach (var entrant in allEntrants)
            {
                viewModel.Add(new AssignedEntrantData
                {
                    EntrantID = entrant.EntrantID,
                    Name = entrant.Name,
                    Thumbnail = entrant.Thumbnail,
                    Assigned = bracketEntrants.Contains(entrant.EntrantID)
                });
            }
            ViewData["Entrants"] = viewModel;
        }

        private void UpdateBracketEntrants(string[] selectedEntrants, Bracket bracketToUpdate)
        {
            if (selectedEntrants == null)
            {
                bracketToUpdate.Contenders = new List<Contender>();
                return;
            }

            var selectedEntrantsHS = new HashSet<string>(selectedEntrants);
            var bracketEntrants = new HashSet<int>(bracketToUpdate.Contenders.Select(c => c.Entrant.EntrantID));
            foreach (var entrant in _context.Entrants)
            {
                if (selectedEntrantsHS.Contains(entrant.EntrantID.ToString()))
                {
                    if (!bracketEntrants.Contains(entrant.EntrantID))
                    {
                        bracketToUpdate.Contenders.Add(new Contender { BracketID = bracketToUpdate.BracketID, EntrantID = entrant.EntrantID, Eliminated = false, RoundsAlive = 0 });
                    }
                }
                else
                {

                    if (bracketEntrants.Contains(entrant.EntrantID))
                    {
                        Contender contenderToRemove = bracketToUpdate.Contenders.SingleOrDefault(i => i.EntrantID == entrant.EntrantID);
                        _context.Remove(contenderToRemove);
                    }
                }
            }
        }

        // GET: Brackets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .SingleOrDefaultAsync(m => m.BracketID == id);
            if (bracket == null)
            {
                return NotFound();
            }

            return View(bracket);
        }

        // POST: Brackets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bracket = await _context.Brackets
                .Include(b => b.Contenders)
                .Include(b => b.BingoBoards)
                .SingleOrDefaultAsync(m => m.BracketID == id);

            _context.Brackets.Remove(bracket);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BracketExists(int id)
        {
            return _context.Brackets.Any(e => e.BracketID == id);
        }
    }
}

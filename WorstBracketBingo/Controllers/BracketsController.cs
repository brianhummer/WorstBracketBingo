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

        // GET: Brackets/Manage/5
        public async Task<IActionResult> Manage(int? id, int? round)
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
            bracketViewModel.VoteLink = bracket.VoteLink;
            bracketViewModel.ResultsLink = bracket.ResultsLink;
            bracketViewModel.Title = bracket.Title;
            bracketViewModel.NumRounds = bracket.Rounds.Count;
            bracketViewModel.Round = bracket.Rounds.SingleOrDefault(r => r.RoundNumber == selectedRound);
            bracketViewModel.Finished = bracket.Finished;

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
        public async Task<IActionResult> Create(Bracket bracket)
        {
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
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracketToUpdate = await _context.Brackets
                .Include(i => i.Contenders)
                    .ThenInclude(i => i.Entrant)
                .SingleOrDefaultAsync(m => m.BracketID == id);

            if (await TryUpdateModelAsync<Bracket>(bracketToUpdate, "", i => i.Title, i => i.VoteLink, i => i.ResultsLink))
            {
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
                if(bracketToUpdate.Rounds.Count > 0)
                {
                    await ProcessRoundResults(bracketToUpdate);
                }

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
                return RedirectToAction("Results",new { id = id });
            }

            return View(bracketToUpdate);
        }

        public async Task<IActionResult> FinishBracket(int? id)
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
                if (bracketToUpdate.Rounds.Count > 0)
                {
                    await ProcessRoundResults(bracketToUpdate);
                }

                //AddNewRound(bracketToUpdate);
                bracketToUpdate.Finished = true;

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
                return RedirectToAction("Results", new { id = id });
            }

            return View(bracketToUpdate);
        }

        // GET: Brackets/Results/5
        public async Task<IActionResult> Results(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .Include(b => b.BingoBoards)
                    .ThenInclude(b => b.Bingos)
                    .ThenInclude(b => b.Round)
                .Include(b => b.BingoBoards)
                    .ThenInclude(b => b.BoardPieces)
                    .ThenInclude(b => b.Contender)
                    .ThenInclude(b => b.Entrant)
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

            int firstBingoRound = int.MaxValue;
            int lastBingoRound = 0;
            bool noBingo = false;

            foreach (var board in bracket.BingoBoards)
            {
                board.Bingos = board.Bingos.OrderBy(b => b.Round.RoundNumber).ToList();
                if (board.Bingos.Count == 0)
                {
                    noBingo = true;
                }
                else
                {
                    var bingoRound = board.Bingos.First().Round.RoundNumber;
                    if (bingoRound < firstBingoRound)
                    {
                        firstBingoRound = bingoRound;
                    }
                    if (bingoRound > lastBingoRound)
                    {
                        lastBingoRound = bingoRound;
                    }
                }
            }

            var boardsViewModel = new BoardsViewModel();
            boardsViewModel.Bracket = bracket;
            boardsViewModel.FirstBingos = await GetBingosByRound(bracket.BracketID, firstBingoRound);
            if(noBingo == false)
                boardsViewModel.LastBingos = await GetBingosByRound(bracket.BracketID, lastBingoRound);

            if(boardsViewModel.LastBingos != null)
            {
                for (int i = 0; i < boardsViewModel.FirstBingos.Count; i++)
                {
                    var boardId = boardsViewModel.FirstBingos[i].BingoBoardId;
                    for (int j = boardsViewModel.LastBingos.Count - 1; j >= 0; j--)
                    {
                        if (boardsViewModel.LastBingos[j].BingoBoardId == boardId)
                            boardsViewModel.LastBingos.RemoveAt(j);
                    }
                }
            }

            var firstRoundElimination = int.MaxValue;
            foreach (var board in bracket.BingoBoards)
            {
                board.BoardPieces = board.BoardPieces.OrderBy(b => b.BoardPosition).ToList();

                if(board.RoundEliminated != -1 && board.RoundEliminated < firstRoundElimination)
                {
                    firstRoundElimination = board.RoundEliminated;
                }
            }


            var firstElims = from b in bracket.BingoBoards
                             where b.RoundEliminated == firstRoundElimination
                             select b.Title;

            boardsViewModel.FirstElimination = new FirstEliminationViewModel
            {
                BracketTitles = firstElims.ToList(),
                RoundNumber = firstRoundElimination
            };

            boardsViewModel.CurrentRound = bracket.Rounds.Count == 0 ? -1 : bracket.Rounds.Max(r => r.RoundNumber);

            for(var i = boardsViewModel.CurrentRound; i >= 0; i--)
            {
                var round = bracket.Rounds.Single(r => r.RoundNumber == i);
                if(round.RoundContenders.Count > 0)
                {
                    List<Entrant> entrants = new List<Entrant>();
                    foreach (var roundContender in round.RoundContenders)
                    {
                        entrants.Add(roundContender.Contender.Entrant);
                    }

                    boardsViewModel.LastContenders = entrants;
                    break;
                }
            }

            return View(boardsViewModel);
        }

        private async Task<IList<Bingo>> GetBingosByRound(int bracketId, int roundNumber)
        {
            var bingos = await _context.Bingos
                .Include(b => b.Round)
                .Include(b => b.BingoBoard)
                .Where(b => b.BracketId == bracketId && b.Round.RoundNumber == roundNumber)
                .AsNoTracking()
                .ToListAsync();

            return bingos;
        }

        private async Task ProcessRoundResults(Bracket bracket)
        {
            var boards = await _context.BingoBoards
                .Include(b => b.BoardPieces)
                    .ThenInclude(b => b.Contender)
                .Where(b => b.BracketID == bracket.BracketID)
                .ToListAsync();

            for (int i = 0; i < boards.Count; i++)
            {
                boards[i].BoardPieces = boards[i].BoardPieces.OrderBy(b => b.BoardPosition).ToList();
            }

            var numCol = 5;
            var numRow = 5;

            for(int i = 0; i < boards.Count; i++)
            {
                // check for row bingos
                for (int c = 0; c < numCol; c++)
                {
                    var horizontalBingo = true;
                    for (int r = 0; r < numRow; r++)
                    {
                        var index = c * numCol + r;
                        if (boards[i].BoardPieces[index].Contender.Eliminated == false)
                        {
                            horizontalBingo = false;
                            break;
                        }
                    }

                    if(horizontalBingo == true)
                    {
                        //add bingo
                        var bingo = new Bingo
                        {
                            BracketId = bracket.BracketID,
                            BingoBoardId = boards[i].BingoBoardID,
                            RoundId = bracket.Rounds.OrderBy(x => x.RoundNumber).Last().RoundID,
                            Label = "Row " + (c + 1)
                        };

                        if(!bingoExists(boards[i].BingoBoardID, bingo.Label))
                        {
                            _context.Add(bingo);
                        }
                    }
                }

                // check for col bingos
                for (int r = 0; r < numRow; r++)
                {
                    var verticalBingo = true;
                    for (int c = 0; c < numCol; c++)
                    {
                        var index = c * numCol + r;
                        if (boards[i].BoardPieces[index].Contender.Eliminated == false)
                        {
                            verticalBingo = false;
                            break;
                        }
                    }

                    if (verticalBingo == true)
                    {
                        //add bingo
                        var bingo = new Bingo
                        {
                            BracketId = bracket.BracketID,
                            BingoBoardId = boards[i].BingoBoardID,
                            RoundId = bracket.Rounds.OrderBy(x => x.RoundNumber).Last().RoundID,
                            Label = "Column " + (r + 1)
                        };

                        if (!bingoExists(boards[i].BingoBoardID, bingo.Label))
                        {
                            _context.Add(bingo);
                        }
                    }
                }

                // check for diag top left to bot right bingos
                var diagonalBingo = true;
                for (int j = 0; j < numRow; j++)
                {
                    var index = j + j * numCol;
                    if (boards[i].BoardPieces[index].Contender.Eliminated == false)
                    {
                        diagonalBingo = false;
                        break;
                    }
                }
                if(diagonalBingo == true)
                {
                    //add bingo
                    var bingo = new Bingo
                    {
                        BracketId = bracket.BracketID,
                        BingoBoardId = boards[i].BingoBoardID,
                        RoundId = bracket.Rounds.OrderBy(x => x.RoundNumber).Last().RoundID,
                        Label = "Diagonal Top Left to Bottom Right"
                    };

                    if (!bingoExists(boards[i].BingoBoardID, bingo.Label))
                    {
                        _context.Add(bingo);
                    }
                }

                // check for diag top left to bot right bingos
                diagonalBingo = true;
                for (int j = 0; j < numRow; j++)
                {
                    //4 - 0, 9 - 1, 14 - 2, 19 - 3, 24 - 4
                    var index = numCol * j + (numCol - 1) - j;

                    if (boards[i].BoardPieces[index].Contender.Eliminated == false)
                    {
                        diagonalBingo = false;
                        break;
                    }

                }
                if(diagonalBingo == true)
                {
                    //add bingo
                    var bingo = new Bingo
                    {
                        BracketId = bracket.BracketID,
                        BingoBoardId = boards[i].BingoBoardID,
                        RoundId = bracket.Rounds.OrderBy(x => x.RoundNumber).Last().RoundID,
                        Label = "Diagonal Top Left to Bottom Right"
                    };

                    if (!bingoExists(boards[i].BingoBoardID, bingo.Label))
                    {
                        _context.Add(bingo);
                    }
                }
            }

            // check round eliminated
            for (int i = 0; i < boards.Count; i++)
            {
                if(boards[i].RoundEliminated <= -1)
                {
                    var isEliminated = true;
                    for (int j = 0; j < boards[i].BoardPieces.Count; j++)
                    {
                        if (!boards[i].BoardPieces[j].Contender.Eliminated)
                        {
                            isEliminated = false;
                            break;
                        }
                    }

                    if (isEliminated == true)
                    {
                        boards[i].RoundEliminated = bracket.Rounds.Max(r => r.RoundNumber);
                    }
                }
            }

            // add round alive for contenders
            foreach (var contender in bracket.Contenders)
            {
                if (!contender.Eliminated)
                {
                    contender.RoundsAlive++;
                }
            }

            // update board contender rounds alive
            for (int i = 0; i < boards.Count; i++)
            {
                var roundsAlive = 0;
                for (var j = 0; j < boards[i].BoardPieces.Count; j++)
                {
                    roundsAlive += boards[i].BoardPieces[j].Contender.RoundsAlive;
                }

                boards[i].ContenderRoundsAlive = roundsAlive;
            }

            await _context.SaveChangesAsync();
        }

        private bool bingoExists(int boardId, string label)
        {
            var existingBingo = _context.Bingos
                .Where(b => b.BingoBoardId == boardId && b.Label == label)
                .AsNoTracking()
                .Any();

            return existingBingo;
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

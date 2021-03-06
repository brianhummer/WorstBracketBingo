﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models.BracketBingoViewModels
{
    public class BoardsViewModel
    {
        public IList<Bingo> FirstBingos { get; set; }
        public IList<Bingo> LastBingos { get; set; }
        public IList<Entrant> LastContenders { get; set; }
        public FirstEliminationViewModel FirstElimination { get; set; }
        public Bracket Bracket { get; set; }
        public int CurrentRound { get; set; }
        

    }
}

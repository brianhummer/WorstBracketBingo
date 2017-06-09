﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class Bracket
    {
        public int BracketID { get; set; }
        public string Title { get; set; }
        public int Size { get; set; }

        public ICollection<BingoBoard> BingoBoards;
    }
}
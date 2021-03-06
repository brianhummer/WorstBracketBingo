﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class BoardPiece
    {
        public int BoardPieceID { get; set; }
        public int ContenderID { get; set; }
        public int BingoBoardID { get; set; }
        public int BoardPosition { get; set; }

        public virtual Contender Contender { get; set; }
        public virtual BingoBoard BingoBoard { get; set; }
    }
}

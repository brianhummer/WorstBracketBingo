using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class BoardPiece
    {
        public int BoardPieceID { get; set; }
        public int EntrantID { get; set; }
        public int BingoBoardID { get; set; }
        public bool Eliminated { get; set; }
        public int RoundsAlive { get; set; }
        public int BoardPosition { get; set; }

        public Entrant Entrant { get; set; }
        public BingoBoard BingoBoard { get; set; }
    }
}

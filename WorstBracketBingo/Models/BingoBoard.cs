using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class BingoBoard
    {
        public int BingoBoardID { get; set; }
        public int BracketID { get; set; }
        public int Title { get; set; }

        public ICollection<BoardPiece> BoardPieces;
    }
}

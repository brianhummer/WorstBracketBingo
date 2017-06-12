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
        public string Title { get; set; }

        public virtual Bracket Bracket { get; set; }
        public virtual ICollection<BoardPiece> BoardPieces { get; set; }
    }
}

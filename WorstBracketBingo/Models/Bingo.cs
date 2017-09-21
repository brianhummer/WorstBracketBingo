using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class Bingo
    {
        public int Id { get; set; }
        public int BingoBoardId { get; set; }
        public int BracketId { get; set; }
        public int RoundId { get; set; }
        public string Label { get; set; }

        public Bracket Bracket { get; set; }
        public BingoBoard BingoBoard { get; set; }
        public Round Round { get; set; }
    }
}

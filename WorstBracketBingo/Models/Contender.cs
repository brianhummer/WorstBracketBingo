using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class Contender
    {
        public int ContenderID { get; set; }
        public int EntrantID { get; set; }
        public int BracketID { get; set; }
        public bool Eliminated { get; set; }
        public int RoundsAlive { get; set; }

        public Entrant Entrant { get; set; }
        public Bracket Bracket { get; set; }
    }
}

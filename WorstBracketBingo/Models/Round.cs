using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class Round
    {
        public int RoundID { get; set; }
        public int RoundNumber { get; set; }

        public ICollection<RoundContender> RoundContenders { get; set; }
        public List<Bingo> Bingos { get; set; }
    }
}

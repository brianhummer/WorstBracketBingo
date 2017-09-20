using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class Bracket
    {
        public int BracketID { get; set; }
        public string Title { get; set; }
        public string VoteLink { get; set; }
        public string ResultsLink { get; set; }
        public bool Finished { get; set; }

        public virtual ICollection<Round> Rounds { get; set; }
        public virtual ICollection<Contender> Contenders { get; set; }
        public virtual ICollection<BingoBoard> BingoBoards { get; set; }
        public IList<Bingo> Bingos { get; set; }
    }
}

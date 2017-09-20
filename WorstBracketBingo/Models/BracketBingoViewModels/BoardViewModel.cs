using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models.BracketBingoViewModels
{
    public class BoardViewModel
    {
        public IList<Bingo> FirstBingos { get; set; }
        public IList<Bingo> LastBingos { get; set; }
        public Bracket Bracket { get; set; }
        

    }
}

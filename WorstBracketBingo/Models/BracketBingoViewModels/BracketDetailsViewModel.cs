using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models.BracketBingoViewModels
{
    public class BracketDetailsViewModel
    {
        public int BracketID { get; set; }
        public string VoteLink { get; set; }
        public string ResultsLink { get; set; }
        public bool Finished { get; set; }
        public string Title { get; set; }
        public int NumRounds { get; set; }
        public Round Round { get; set; }
    }
}

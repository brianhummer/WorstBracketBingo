using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models.BracketBingoViewModels
{
    public class AssignedRoundData
    {
        public int RoundID { get; set; }
        public int RoundNumber { get; set; }
        public bool Assigned { get; set; }

        public ICollection<Contender> Contenders { get; set; }
    }
}

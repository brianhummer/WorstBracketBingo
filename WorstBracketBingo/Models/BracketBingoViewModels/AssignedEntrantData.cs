using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models.BracketBingoViewModels
{
    public class AssignedEntrantData
    {
        public int EntrantID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models.BracketBingoViewModels
{
    public class AssignedContenderData
    {
        public int ContenderID { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public bool Assigned { get; set; }
    }
}

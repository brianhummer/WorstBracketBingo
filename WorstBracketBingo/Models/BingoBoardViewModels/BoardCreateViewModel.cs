using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models.BingoBoardViewModels
{
    public class BoardCreateViewModel
    {
        public int BracketID { get; set; }
        [Required]
        public string Title { get; set; }

        public IList<int> Entrants { get; set; }
    }
}

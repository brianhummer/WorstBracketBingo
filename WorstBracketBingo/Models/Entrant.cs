using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorstBracketBingo.Models
{
    public class Entrant
    {
        public int EntrantID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Tag { get; set; }
        [Required]
        public string Thumbnail { get; set; }
    }
}

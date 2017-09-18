namespace WorstBracketBingo.Models
{
    public class RoundContender
    {
        public int RoundID { get; set; }
        public int ContenderID { get; set; }

        public Round Round { get; set; }
        public Contender Contender { get; set; }
    }
}

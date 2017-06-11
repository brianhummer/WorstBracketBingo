using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorstBracketBingo.Models;

namespace WorstBracketBingo.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BingoContext context)
        {
            context.Database.EnsureCreated();

            if(context.Entrants.Any())
            {
                return;
            }

            var entrants = new Entrant[]
            {
                new Entrant{ Name="Holo", Tag="Spice & Wolf", Thumbnail="defaultThumb.jpg"},
                new Entrant{ Name="Rin Tohsaka", Tag="FSN UBW", Thumbnail="defaultThumb.jpg"},
                new Entrant{ Name="Kazusa Touma", Tag="White Album 2", Thumbnail="defaultThumb.jpg"},
                new Entrant{ Name="Asuka Langley", Tag="Evangelion", Thumbnail="defaultThumb.jpg"}
            };

            foreach (Entrant e in entrants)
            {
                context.Entrants.Add(e);
            }
            context.SaveChanges();

            var brackets = new Bracket[]
            {
                new Bracket{ Title="Best Girl 4: A Certain Salty Railgun!", Size=512, Round=0}
            };

            foreach (Bracket b in brackets)
            {
                context.Brackets.Add(b);
            }
            context.SaveChanges();

            var contenders = new Contender[]
            {
                new Contender{ BracketID=1, EntrantID=1, Eliminated=false, RoundsAlive=0},
                new Contender{ BracketID=1, EntrantID=2, Eliminated=false, RoundsAlive=0}
            };

            foreach (Contender c in contenders)
            {
                context.Contenders.Add(c);
            }
            context.SaveChanges();

        }
    }
}

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
            /* 1*/   new Entrant{ Name="Holo", Tag="Spice & Wolf", Thumbnail="ShitTasteBingo-Holo.jpg"},
            /* 2*/   new Entrant{ Name="Rin Tohsaka", Tag="FSN UBW", Thumbnail="ShitTasteBingo-RinTohsaka.jpg"},
            /* 3*/   new Entrant{ Name="Kazusa Touma", Tag="White Album 2", Thumbnail="defaultThumb.jpg"},
            /* 4*/   new Entrant{ Name="Asuka Langley", Tag="Evangelion", Thumbnail="defaultThumb.jpg"},
            /* 5*/   new Entrant{ Name="Nodoka Miyazaki", Tag="Mahou Sensei Negima!", Thumbnail="ShitTasteBingo-NodokaMiyazaki.jpg"},
            /* 6*/   new Entrant{ Name="Rosette Christopher", Tag="Chrno Crusade", Thumbnail="ShitTasteBingo-RosetteChristopher.jpg"},
            /* 7*/   new Entrant{ Name="Kuesu Jinguuji", Tag="Omamori Himari", Thumbnail="ShitTasteBingo-KuesuJinguuji.jpg"},
            /* 8*/   new Entrant{ Name="Mitsuru Kirijo", Tag="Persona 3", Thumbnail="ShitTasteBingo-MitsuruKirijo.jpg"},
            /* 9*/   new Entrant{ Name="Momoko Kuzuryu", Tag="Sumomomo Momomo", Thumbnail="ShitTasteBingo-MomokoKuzuryu.jpg"},
            /*10*/   new Entrant{ Name="Nelliel Tu Oderschvank", Tag="Bleach", Thumbnail="ShitTasteBingo-NelTu.jpg"},
            /*11*/   new Entrant{ Name="Katsura Hinagaku", Tag="Hayate the Combat Butler", Thumbnail="ShitTasteBingo-KatsuraHinagiku.jpg"},
            /*12*/   new Entrant{ Name="Elsie", Tag="The World God Only Knows", Thumbnail="ShitTasteBingo-Elsie.jpg"},
            /*13*/   new Entrant{ Name="Rem", Tag="Re:Zero", Thumbnail="ShitTasteBingo-Rem.jpg"},
            /*14*/   new Entrant{ Name="Chisame Hasegawa", Tag="Mahou Sensei Negima!", Thumbnail="ShitTasteBingo-ChisameHasegawa.jpg"},
            /*15*/   new Entrant{ Name="Shana", Tag="Shakugan no Shana", Thumbnail="ShitTasteBingo-Shana.jpg"},
            /*16*/   new Entrant{ Name="Yui Yuigahama", Tag="Snafu", Thumbnail="ShitTasteBingo-YuiYuigahama.jpg"},
            /*17*/   new Entrant{ Name="Ryoko Hakubi", Tag="Tenchi Muyo", Thumbnail="ShitTasteBingo-RyokoHakubi.jpg"},
            /*18*/   new Entrant{ Name="Le Blanc de La Vallière", Tag="Louise Françoise(383) Zero no Tsukaima", Thumbnail="ShitTasteBingo-LouiseTheZero.jpg"},
            /*19*/   new Entrant{ Name="Moka Akashiya", Tag="Rosario + Vampire", Thumbnail="ShitTasteBingo-MokaAkashiya.jpg"},
            /*20*/   new Entrant{ Name="Morgiana", Tag="Magi", Thumbnail="ShitTasteBingo-Morgiana.jpg"},
            /*21*/   new Entrant{ Name="Simca", Tag="Air Gear!", Thumbnail="ShitTasteBingo-Simca.jpg"},
            /*22*/   new Entrant{ Name="Shirahoshi", Tag="One Piece", Thumbnail="ShitTasteBingo-Shirahoshi.jpg"},
            /*23*/   new Entrant{ Name="Rory Mercury", Tag="Gate", Thumbnail="ShitTasteBingo-RoryMercury.jpg"},
            /*24*/   new Entrant{ Name="Hestia", Tag="Danmachi", Thumbnail="ShitTasteBingo-Hestia.jpg"},
            /*25*/   new Entrant{ Name="Orihime Inoue", Tag="Bleach", Thumbnail="ShitTasteBingo-OrihimeInoue.jpg"},
            /*26*/   new Entrant{ Name="Hinata Hyuuga", Tag="Naruto", Thumbnail="ShitTasteBingo-HinataHyuuga.jpg"},
            /*27*/   new Entrant{ Name="Yukikio Amagi", Tag="Persona 4", Thumbnail="ShitTasteBingo-YukikioAmagi.jpg"},
            };

            foreach (Entrant e in entrants)
            {
                context.Entrants.Add(e);
            }
            context.SaveChanges();

            var brackets = new Bracket[]
            {
                new Bracket{ Title="Best Girl 4: A Certain Salty Railgun!"}
            };

            foreach (Bracket b in brackets)
            {
                context.Brackets.Add(b);
            }
            context.SaveChanges();

            for(var i = 0; i < entrants.Length; i++)
            {
                context.Contenders.Add(new Contender { BracketID = 1, EntrantID = i + 1, Eliminated = false, RoundsAlive = 0 });
            }
            context.SaveChanges();


            var boards = new BingoBoard[]
            {
                new BingoBoard{ BracketID = 1, Title = "Brian's Top of the Trash Bin"},
                new BingoBoard{ BracketID = 1, Title = "Tony's Trash Heap of Garbage"},
                new BingoBoard{ BracketID = 1, Title = "Matt's One True Trash"},
            };

            foreach (BingoBoard b in boards)
            {
                context.BingoBoards.Add(b);
            }
            context.SaveChanges();

            var boardPieces = new BoardPiece[]
            {
                new BoardPiece{ BingoBoardID = 2, ContenderID = 15, BoardPosition = 1},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 2, BoardPosition = 2},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 18, BoardPosition = 3},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 11, BoardPosition = 4},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 12, BoardPosition = 5},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 21, BoardPosition = 6},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 22, BoardPosition = 7},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 10, BoardPosition = 8},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 13, BoardPosition = 9},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 7, BoardPosition = 10},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 20, BoardPosition = 11},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 14, BoardPosition = 12},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 5, BoardPosition = 13},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 26, BoardPosition = 14},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 16, BoardPosition = 15},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 6, BoardPosition = 16},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 9, BoardPosition = 17},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 1, BoardPosition = 18},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 24, BoardPosition = 19},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 17, BoardPosition = 20},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 23, BoardPosition = 21},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 25, BoardPosition = 22},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 27, BoardPosition = 23},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 8, BoardPosition = 24},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 19, BoardPosition = 25}
            };
            foreach (BoardPiece b in boardPieces)
            {
                context.BoardPieces.Add(b);
            }
            context.SaveChanges();
        }
    }
}

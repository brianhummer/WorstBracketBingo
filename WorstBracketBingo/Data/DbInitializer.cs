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
                new Entrant{ Name="Holo", Tag="Spice & Wolf", Thumbnail="ShitTasteBingo-Holo.jpg"},
                new Entrant{ Name="Rin Tohsaka", Tag="FSN UBW", Thumbnail="ShitTasteBingo-RinTohsaka.jpg"},
                new Entrant{ Name="Kazusa Touma", Tag="White Album 2", Thumbnail="ShitTasteBingo-KazusaTouma.jpg"},
                new Entrant{ Name="Asuka Langley", Tag="Evangelion", Thumbnail="ShitTasteBingo-AsukaLangley.jpg"},
                new Entrant{ Name="Nodoka Miyazaki", Tag="Mahou Sensei Negima!", Thumbnail="ShitTasteBingo-NodokaMiyazaki.jpg"},
                new Entrant{ Name="Rosette Christopher", Tag="Chrno Crusade", Thumbnail="ShitTasteBingo-RosetteChristopher.jpg"},
                new Entrant{ Name="Kuesu Jinguuji", Tag="Omamori Himari", Thumbnail="ShitTasteBingo-KuesuJinguuji.jpg"},
                new Entrant{ Name="Mitsuru Kirijo", Tag="Persona 3", Thumbnail="ShitTasteBingo-MitsuruKirijo.jpg"},
                new Entrant{ Name="Momoko Kuzuryu", Tag="Sumomomo Momomo", Thumbnail="ShitTasteBingo-MomokoKuzuryu.jpg"},
                new Entrant{ Name="Nelliel Tu Oderschvank", Tag="Bleach", Thumbnail="ShitTasteBingo-NelTu.jpg"},
                new Entrant{ Name="Katsura Hinagaku", Tag="Hayate the Combat Butler", Thumbnail="ShitTasteBingo-KatsuraHinagiku.jpg"},
                new Entrant{ Name="Elsie", Tag="The World God Only Knows", Thumbnail="ShitTasteBingo-Elsie.jpg"},
                new Entrant{ Name="Rem", Tag="Re:Zero", Thumbnail="ShitTasteBingo-Rem.jpg"},
                new Entrant{ Name="Chisame Hasegawa", Tag="Mahou Sensei Negima!", Thumbnail="ShitTasteBingo-ChisameHasegawa.jpg"},
                new Entrant{ Name="Shana", Tag="Shakugan no Shana", Thumbnail="ShitTasteBingo-Shana.jpg"},
                new Entrant{ Name="Yui Yuigahama", Tag="Snafu", Thumbnail="ShitTasteBingo-YuiYuigahama.jpg"},
                new Entrant{ Name="Ryoko Hakubi", Tag="Tenchi Muyo", Thumbnail="ShitTasteBingo-RyokoHakubi.jpg"},
                new Entrant{ Name="Le Blanc de La Vallière", Tag="Louise Françoise(383) Zero no Tsukaima", Thumbnail="ShitTasteBingo-LouiseTheZero.jpg"},
                new Entrant{ Name="Moka Akashiya", Tag="Rosario + Vampire", Thumbnail="ShitTasteBingo-MokaAkashiya.jpg"},
                new Entrant{ Name="Morgiana", Tag="Magi", Thumbnail="ShitTasteBingo-Morgiana.jpg"},
                new Entrant{ Name="Simca", Tag="Air Gear!", Thumbnail="ShitTasteBingo-Simca.jpg"},
                new Entrant{ Name="Shirahoshi", Tag="One Piece", Thumbnail="ShitTasteBingo-Shirahoshi.jpg"},
                new Entrant{ Name="Rory Mercury", Tag="Gate", Thumbnail="ShitTasteBingo-RoryMercury.jpg"},
                new Entrant{ Name="Hestia", Tag="Danmachi", Thumbnail="ShitTasteBingo-Hestia.jpg"},
                new Entrant{ Name="Orihime Inoue", Tag="Bleach", Thumbnail="ShitTasteBingo-OrihimeInoue.jpg"},
                new Entrant{ Name="Hinata Hyuuga", Tag="Naruto", Thumbnail="ShitTasteBingo-HinataHyuuga.jpg"},
                new Entrant{ Name="Yukikio Amagi", Tag="Persona 4", Thumbnail="ShitTasteBingo-YukikioAmagi.jpg"},
                new Entrant{ Name="Sento", Tag="Amagi Brilliant Park", Thumbnail="ShitTasteBingo-Sento.jpg"},
                new Entrant{ Name="Birdy Cephon Altera", Tag="Birdy the Mighty Decode", Thumbnail="ShitTasteBingo-BirdyCephonAltera.jpg"},
                new Entrant{ Name="Kyou Fujibayashi", Tag="Clannad", Thumbnail="ShitTasteBingo-KyouFujibayashi.jpg"},
                new Entrant{ Name="Tomoyo Sakagami", Tag="Clannad", Thumbnail="ShitTasteBingo-TomoyoSakagami.jpg"},
                new Entrant{ Name="Mio Akiyama", Tag="K-On", Thumbnail="ShitTasteBingo-MioAkiyama.jpg"},
                new Entrant{ Name="Yuuko Kanoe", Tag="Dusk Maiden of Amnesia", Thumbnail="ShitTasteBingo-YuukoKanoe.jpg"},
                new Entrant{ Name="Winry Rockbell", Tag="FMAB", Thumbnail="ShitTasteBingo-WinryRockbell.jpg"},
                new Entrant{ Name="Yume", Tag="Grimgar", Thumbnail="ShitTasteBingo-Yume.jpg"},
                new Entrant{ Name="Sakura Chiyo", Tag="Nozaki-kun", Thumbnail="ShitTasteBingo-SakuraChiyo.jpg"},
                new Entrant{ Name="Himeko Inaba", Tag="Kokoro Connect", Thumbnail="ShitTasteBingo-HimekoInaba.jpg"},
                new Entrant{ Name="Haru Nishimura", Tag="Xam'd Lost Memories", Thumbnail="ShitTasteBingo-HaruNishimura.jpg"},
                new Entrant{ Name="Marida Cruz", Tag="Gundam Unicorn", Thumbnail="ShitTasteBingo-MaridaCruz.jpg"},
                new Entrant{ Name="Revy", Tag="Black Lagoon", Thumbnail="ShitTasteBingo-Revy.jpg"},
                new Entrant{ Name="Satomi Murano", Tag="Parasyte", Thumbnail="ShitTasteBingo-SatomiMurano.jpg"},
                new Entrant{ Name="Mitsuki Hayase", Tag="Rumbling Hearts", Thumbnail="ShitTasteBingo-MitsukiHayase.jpg"},
                new Entrant{ Name="Shiki Ryougi", Tag="Garden of Sinners", Thumbnail="ShitTasteBingo-ShikiRyougi.jpg"},
                new Entrant{ Name="Izumi Nase", Tag="Beyond the Boundary", Thumbnail="ShitTasteBingo-IzumiNase.jpg"},
                new Entrant{ Name="Nanami Aoyama", Tag="Pet Girl of Sakurasou", Thumbnail="ShitTasteBingo-NanamiAoyama.jpg"},
                new Entrant{ Name="Hakaze Kusaribe", Tag="Blast of Tempest", Thumbnail="ShitTasteBingo-HakazeKusaribe.jpg"},
                new Entrant{ Name="Megumi Kato", Tag="Saekano", Thumbnail="ShitTasteBingo-MegumiKato.jpg"},
                new Entrant{ Name="Mai Minakami", Tag="Nichijou", Thumbnail="ShitTasteBingo-MaiMinakami.jpg"},
                new Entrant{ Name="Shidare Hotaru", Tag="Dagashi Kashi", Thumbnail="ShitTasteBingo-ShidareHotaru.jpg"},
                new Entrant{ Name="Saya Endou", Tag="Dagashi Kashi", Thumbnail="ShitTasteBingo-SayaEndou.jpg"},
                new Entrant{ Name="Hange Zoe", Tag="Attack on Titan", Thumbnail="ShitTasteBingo-HangeZoe.jpg"},
                new Entrant{ Name="Haruhara Haruko", Tag="FLCL", Thumbnail="ShitTasteBingo-HaruharaHaruko.jpg"},
                new Entrant{ Name="Aoi Sakurai", Tag="Rail Wars!", Thumbnail="ShitTasteBingo-AoiSakurai.jpg"},
                new Entrant{ Name="Virgo", Tag="Fairy Tail", Thumbnail="ShitTasteBingo-Virgo.jpg"},
                new Entrant{ Name="Asui Tsuyu", Tag="My Hero Academia", Thumbnail="ShitTasteBingo-AsuiTsuyu.jpg"},
                new Entrant{ Name="Miria Harvent", Tag="Baccano!", Thumbnail="ShitTasteBingo-MiriaHarvent.jpg"},
                new Entrant{ Name="Scarlet", Tag="Space Dandy", Thumbnail="ShitTasteBingo-Scarlet.jpg"},
                new Entrant{ Name="Levi Kazama", Tag="Trinity Seven", Thumbnail="ShitTasteBingo-LeviKazama.jpg"},
                new Entrant{ Name="Mayuri Shiina", Tag="Steins; Gate", Thumbnail="ShitTasteBingo-MayuriShiina.jpg"},
                new Entrant{ Name="Megumi Shimizu", Tag="Shiki", Thumbnail="ShitTasteBingo-MegumiShimizu.jpg"},
                new Entrant{ Name="Meika Daihatsu", Tag="Punch Line", Thumbnail="ShitTasteBingo-MeikaDaihatsu.jpg"},
                new Entrant{ Name="Milly Thompson", Tag="Trigun", Thumbnail="ShitTasteBingo-MillyThompson.jpg"},
                new Entrant{ Name="Parfet Balblair", Tag="Vandread", Thumbnail="ShitTasteBingo-ParfetBalblair.jpg"},
                new Entrant{ Name="Ryouko Ookami", Tag="Ookami-san and her Seven Companions", Thumbnail="ShitTasteBingo-RyoukoOokami.jpg"},
                new Entrant{ Name="Sensei", Tag="Denki-gai", Thumbnail="ShitTasteBingo-Sensei.jpg"},
                new Entrant{ Name="Haruhi Fujioka", Tag="Ouran High School Host Club", Thumbnail="ShitTasteBingo-HaruhiFujioka.jpg"},
                new Entrant{ Name="Yakumo Tsukamoto", Tag="School Rumble", Thumbnail="ShitTasteBingo-YakumoTsukamoto.jpg"},
                new Entrant{ Name="Jun Shiomi", Tag="Food Wars", Thumbnail="ShitTasteBingo-JunShiomi.jpg"},
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
                new BoardPiece{ BingoBoardID = 2, ContenderID = 14, BoardPosition = 1},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 16, BoardPosition = 2},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 32, BoardPosition = 3},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 10, BoardPosition = 4},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 11, BoardPosition = 5},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 29, BoardPosition = 6},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 28, BoardPosition = 7},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 9, BoardPosition = 8},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 12, BoardPosition = 9},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 6, BoardPosition = 10},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 30, BoardPosition = 11},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 13, BoardPosition = 12},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 4, BoardPosition = 13},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 25, BoardPosition = 14},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 15, BoardPosition = 15},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 5, BoardPosition = 16},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 8, BoardPosition = 17},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 1, BoardPosition = 18},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 26, BoardPosition = 19},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 17, BoardPosition = 20},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 27, BoardPosition = 21},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 59, BoardPosition = 22},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 7, BoardPosition = 23},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 23, BoardPosition = 24},
                new BoardPiece{ BingoBoardID = 2, ContenderID = 31, BoardPosition = 25},

                new BoardPiece{ BingoBoardID = 1, ContenderID = 21, BoardPosition = 1},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 16, BoardPosition = 2},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 45, BoardPosition = 3},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 41, BoardPosition = 4},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 36, BoardPosition = 5},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 43, BoardPosition = 6},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 34, BoardPosition = 7},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 42, BoardPosition = 8},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 12, BoardPosition = 9},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 22, BoardPosition = 10},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 19, BoardPosition = 11},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 38, BoardPosition = 12},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 1, BoardPosition = 13},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 18, BoardPosition = 14},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 15, BoardPosition = 15},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 2, BoardPosition = 16},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 35, BoardPosition = 17},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 39, BoardPosition = 18},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 44, BoardPosition = 19},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 20, BoardPosition = 20},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 37, BoardPosition = 21},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 40, BoardPosition = 22},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 50, BoardPosition = 23},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 3, BoardPosition = 24},
                new BoardPiece{ BingoBoardID = 1, ContenderID = 33, BoardPosition = 25},

                new BoardPiece{ BingoBoardID = 3, ContenderID = 68, BoardPosition = 1},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 47, BoardPosition = 2},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 54, BoardPosition = 3},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 24, BoardPosition = 4},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 61, BoardPosition = 5},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 58, BoardPosition = 6},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 66, BoardPosition = 7},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 55, BoardPosition = 8},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 60, BoardPosition = 9},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 46, BoardPosition = 10},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 30, BoardPosition = 11},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 57, BoardPosition = 12},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 50, BoardPosition = 13},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 64, BoardPosition = 14},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 67, BoardPosition = 15},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 49, BoardPosition = 16},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 51, BoardPosition = 17},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 39, BoardPosition = 18},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 63, BoardPosition = 19},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 62, BoardPosition = 20},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 52, BoardPosition = 21},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 65, BoardPosition = 22},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 56, BoardPosition = 23},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 48, BoardPosition = 24},
                new BoardPiece{ BingoBoardID = 3, ContenderID = 53, BoardPosition = 25}
            };
            foreach (BoardPiece b in boardPieces)
            {
                context.BoardPieces.Add(b);
            }
            context.SaveChanges();
        }
    }
}

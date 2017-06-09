﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorstBracketBingo.Models;

namespace WorstBracketBingo.Data
{
    public class BingoContext : DbContext
    {
        public BingoContext(DbContextOptions<BingoContext> options) : base(options)
        {

        }

        public DbSet<Entrant> Entrants { get; set; }
        public DbSet<Bracket> Brackets { get; set; }
        public DbSet<BingoBoard> BingoBoards { get; set; }
        public DbSet<BoardPiece> BoardPieces { get; set; }
    }
}
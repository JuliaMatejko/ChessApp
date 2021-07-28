using Microsoft.EntityFrameworkCore;
using ChessApp.Models;
using ChessApp.Models.Chess;
using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<File> Files { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PieceName> PieceNames { get; set; }
        /*public DbSet<Piece> Pieces { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldColumn> FieldColumns { get; set; }
        public DbSet<Pawn> Pawns { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Bishop> Bishops { get; set; }
        public DbSet<Rook> Rooks { get; set; }
        public DbSet<Queen> Queens { get; set; }
        public DbSet<Knight> Knights { get; set; }
        public DbSet<King> Kings { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Game> Games { get; set; }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .HasKey(p => new { p.FileID, p.RankID });
        }

    }
}

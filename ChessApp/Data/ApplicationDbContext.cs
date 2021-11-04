using ChessApp.Models.Chess;
using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Piece> Pieces { get; set; }
        public DbSet<FieldColumn> FieldColumns { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Pawn> Pawns { get; set; }
        public DbSet<Rook> Rooks { get; set; }
        public DbSet<Knight> Knights { get; set; }
        public DbSet<Bishop> Bishops { get; set; }
        public DbSet<Queen> Queens { get; set; }
        public DbSet<King> Kings { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardFile> BoardsFiles { get; set; }
        public DbSet<BoardRank> BoardsRanks { get; set; }
        public DbSet<BoardPosition> BoardsPositions { get; set; }
        public DbSet<BoardFieldColumn> BoardsFieldColumns { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameState> GameStates { get; set; }
        public DbSet<ControlledSquare> ControlledSquares { get; set; }
        public DbSet<NextAvailablePosition> NextAvailablePositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<Position>()
                .HasOne(i => i.Piece)
                .WithOne(p => p.Position)
                .HasForeignKey<Piece>(i => i.PositionID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Position>()
                .HasOne(i => i.Field)
                .WithOne(p => p.Position)
                .HasForeignKey<Field>(i => i.PositionID);

            modelBuilder.Entity<Field>()
                .HasOne(i => i.Content)
                .WithOne(p => p.Field)
                .HasForeignKey<Field>(i => i.PieceID);

            modelBuilder.Entity<BoardFile>()
               .HasKey(k => new { k.GameID, k.FileID });

            modelBuilder.Entity<BoardRank>()
               .HasKey(k => new { k.GameID, k.RankID });

            modelBuilder.Entity<BoardPosition>()
               .HasKey(k => new { k.GameID, k.PositionID });

            modelBuilder.Entity<BoardFieldColumn>()
               .HasKey(k => new { k.GameID, k.FieldColumnID });

            modelBuilder.Entity<GameState>()
                .HasOne(i => i.WhiteKing)
                .WithOne(p => p.GameStateWhiteKing)
                .HasForeignKey<GameState>(i => i.WhiteKingID);

            modelBuilder.Entity<GameState>()
                .HasOne(i => i.BlackKing)
                .WithOne(p => p.GameStateBlackKing)
                .HasForeignKey<GameState>(i => i.BlackKingID);

            modelBuilder.Entity<GameState>()
                .HasOne(i => i.WhitePawnThatCanBeTakenByEnPassantMove)
                .WithOne(p => p.GameStateWhitePawn)
                .HasForeignKey<GameState>(i => i.WhitePawnThatCanBeTakenByEnPassantMoveID);

            modelBuilder.Entity<GameState>()
                .HasOne(i => i.BlackPawnThatCanBeTakenByEnPassantMove)
                .WithOne(p => p.GameStateBlackPawn)
                .HasForeignKey<GameState>(i => i.BlackPawnThatCanBeTakenByEnPassantMoveID);

            modelBuilder.Entity<ControlledSquare>()
               .HasKey(k => new { k.PieceID, k.PositionID });

            modelBuilder.Entity<NextAvailablePosition>()
               .HasKey(k => new { k.PieceID, k.PositionID });
        }
    }
}

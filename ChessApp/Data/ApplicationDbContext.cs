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
            modelBuilder.Entity<Move>()
               .HasKey(k => new { k.GameID, k.MoveID });

            modelBuilder.Entity<Board>()
               .HasKey(k => k.GameID );

            modelBuilder.Entity<GameState>()
               .HasKey(k => k.GameID);

            modelBuilder.Entity<GameState>()
              .HasOne(i => i.WhiteKing)
              .WithOne(i => i.WhiteKingGameState)
              .HasForeignKey<GameState>(k => new { k.GameID, k.WhiteKingID });

            modelBuilder.Entity<GameState>()
              .HasOne(i => i.BlackKing)
              .WithOne(i => i.BlackKingGameState)
              .HasForeignKey<GameState>(k => new { k.GameID, k.BlackKingID });

            modelBuilder.Entity<GameState>()
              .HasOne(i => i.WhitePawnThatCanBeTakenByEnPassantMove)
              .WithOne(i => i.WhitePawnThatCanBeTakenByEnPassantMoveGameState)
              .HasForeignKey<GameState>(k => new { k.GameID, k.WhitePawnThatCanBeTakenByEnPassantMoveID });

            modelBuilder.Entity<GameState>()
              .HasOne(i => i.BlackPawnThatCanBeTakenByEnPassantMove)
              .WithOne(i => i.BlackPawnThatCanBeTakenByEnPassantMoveGameState)
              .HasForeignKey<GameState>(k => new { k.GameID, k.BlackPawnThatCanBeTakenByEnPassantMoveID });

            modelBuilder.Entity<Piece>()
               .HasKey(k => new { k.GameID, k.PieceID });

            modelBuilder.Entity<BoardFile>()
              .HasKey(k => new { k.GameID, k.FileID });

            modelBuilder.Entity<BoardRank>()
               .HasKey(k => new { k.GameID, k.RankID });

            modelBuilder.Entity<BoardPosition>()
               .HasKey(k => new { k.GameID, k.PositionID });

            modelBuilder.Entity<BoardFieldColumn>()
               .HasKey(k => new { k.GameID, k.FieldColumnID });

            modelBuilder.Entity<ControlledSquare>()
               .HasKey(k => new { k.PieceGameID, k.PieceID, k.PositionID });

            modelBuilder.Entity<NextAvailablePosition>()
               .HasKey(k => new { k.PieceGameID, k.PieceID, k.PositionID });

            //

            modelBuilder.Entity<Piece>()
                .HasOne(i => i.Position)
                .WithOne(p => p.Piece)
                .HasForeignKey<Position>(i => new { i.GameID, i.PieceID })
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Position>()
                .HasOne(i => i.Field)
                .WithOne(p => p.Position)
                .HasForeignKey<Field>(i => i.PositionID);

            modelBuilder.Entity<Field>()
                .HasOne(i => i.Content)
                .WithOne(p => p.Field)
                .HasForeignKey<Field>(i => new { i.PieceGameID, i.PieceID });

            modelBuilder.Entity<GameState>()
                .HasMany(i => i.CurrentPlayerPiecesAttackingTheKing)
                .WithOne(p => p.GameState)
                .OnDelete(DeleteBehavior.NoAction);


           
        }
    }
}

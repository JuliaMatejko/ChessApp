using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.Pieces
{
    public abstract class Piece : ICloneable
    {
        public static readonly string[] pieceNames = new string[] { "bb", "bw", "kb", "kw", "nb", "nw", "pb", "pw", "qb", "qw", "rb", "rw" };

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PieceID { get; set; }
        [Required]
        public bool IsWhite { get; set; }
        public static List<PieceName> PieceNames { get; set; }
        [Required]
        public int PositionID { get; set; }
        public Position Position { get; set; }
        [Required]
        [Column("PieceName")]
        [Display(Name = "Piece Name")]
        public string PieceNameID { get; set; }
        public PieceName Name { get; set; }
        [ForeignKey("GameState")]
        public int GameID { get; set; }
        public GameState GameState { get; set; }

        public Field Field { get; set; }
        public HashSet<NextAvailablePosition> NextAvailablePositions { get; set; }
        public HashSet<ControlledSquare> ControlledSquares { get; set; } = new();

        
        public HashSet<NextAvailablePosition> ReturnAvailablePieceMoves()
        {
            int fileIndex = Array.IndexOf(Board.files, Position.FileID);
            int rankIndex = Array.IndexOf(Board.ranks, Position.RankID);
            HashSet<NextAvailablePosition> positions = new();
            positions = ReturnCorrectPieceMoves(fileIndex, rankIndex, GameState.Game.Chessboard, positions);
            return positions;
        }

        protected abstract HashSet<NextAvailablePosition> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions);
        
        public object Clone()
        {
            return this != null ? MemberwiseClone() : null;
        }
    }
}

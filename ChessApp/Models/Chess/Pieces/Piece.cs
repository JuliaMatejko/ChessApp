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

        public Field Field { get; set; }

        public HashSet<Position> NextAvailablePositions { get; set; }

        public HashSet<Position> ControlledSquares { get; set; }

        /*
        public HashSet<string> ReturnAvailablePieceMoves(string currentPosition, Board board)
        {
            int fileIndex = Array.IndexOf(Board.Files, Convert.ToString(currentPosition[0]));
            int rankIndex = Array.IndexOf(Board.Ranks, Convert.ToString(currentPosition[1]));
            HashSet<string> positions = new HashSet<string>();
            positions = ReturnCorrectPieceMoves(fileIndex, rankIndex, board, positions);
            return positions;
        }

        protected abstract HashSet<string> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<string> positions);
        */
        public object Clone()
        {
            return this != null ? MemberwiseClone() : null;
        }
    }
}

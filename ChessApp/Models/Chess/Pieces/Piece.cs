using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess.Pieces
{
    public abstract class Piece : ICloneable
    {
        [Key]
        public int PieceId { get; set; }
        [Required]
        public bool IsWhite { get; set; }
        [Required]
        public PieceName Name { get; set; }
        [Required]
        public Position Position { get; set; }
        [Required]
        public HashSet<Position> NextAvailablePositions { get; set; } = new HashSet<Position>();
        [Required]
        public HashSet<Position> ControlledSquares { get; set; } = new HashSet<Position>();
        [Required]
        public static List<PieceName> PieceNames { get; set; } = AddPieceNames(new string[]{ "pw", "pb", "Rw", "Rb", "Nw", "Nb", "Bw", "Bb", "Qw", "Qb", "Kw", "Kb" });

        protected static List<PieceName> AddPieceNames(string[] pieceNames)
        {
            List<PieceName> list = new List<PieceName>();
            foreach (string name in pieceNames)
            {
                list.Add(new PieceName(name));
            }
            return list;
        }
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

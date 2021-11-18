﻿using ChessApp.Models.Chess.BoardProperties;
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

        [ForeignKey("GameState")]
        public int GameID { get; set; }
        public GameState GameState { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PieceID { get; set; }
        [Required]
        public bool IsWhite { get; set; }
        [Required]
        public int PositionID { get; set; }
        public Position Position { get; set; }
        [Required]
        [Column("PieceName")]
        [Display(Name = "Piece Name")]
        public string PieceNameID { get; set; }
        public PieceName Name { get; set; }

        public Field Field { get; set; }
        public HashSet<NextAvailablePosition> NextAvailablePositions { get; set; }
        public HashSet<ControlledSquare> ControlledSquares { get; set; } = new();

        
        public HashSet<NextAvailablePosition> ReturnAvailablePieceMoves(Board board)
        {
            int fileIndex = GameState.Game.Chessboard.BoardsFiles.IndexOf(GameState.Game.Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == Position.FileID));
            int rankIndex = GameState.Game.Chessboard.BoardsRanks.IndexOf(GameState.Game.Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == Position.RankID));
            HashSet<NextAvailablePosition> positions = new();
            positions = ReturnCorrectPieceMoves(fileIndex, rankIndex, board, positions);
            return positions;
        }

        protected abstract HashSet<NextAvailablePosition> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions);
        
        public object Clone()
        {
            return this != null ? MemberwiseClone() : null;
        }
    }
}

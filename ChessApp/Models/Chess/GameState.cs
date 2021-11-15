﻿using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChessApp.Models.Chess
{
    public class GameState
    {
        [Key]
        public int GameID { get; set; }
        public Game Game { get; set; }

        [ForeignKey("WhiteKing")]
        public int? WhiteKingID { get; set; }
        public King WhiteKing { get; set; }
        [ForeignKey("BlackKing")]
        public int? BlackKingID { get; set; }
        public King BlackKing { get; set; }

        public int? WhitePawnThatCanBeTakenByEnPassantMoveID { get; set; }
        [ForeignKey("WhitePawnThatCanBeTakenByEnPassantMoveID")]
        public Pawn WhitePawnThatCanBeTakenByEnPassantMove { get; set; }
        public int? BlackPawnThatCanBeTakenByEnPassantMoveID { get; set; }
        [ForeignKey("BlackPawnThatCanBeTakenByEnPassantMoveID")]
        public Pawn BlackPawnThatCanBeTakenByEnPassantMove { get; set; }

        public List<Piece> CurrentPlayerPiecesAttackingTheKing { get; set; }
        public enum Sides
        {
            White = 1,
            Black = 0
        }
        public Sides CurrentPlayer { get; set; }
        public bool WhiteKingIsInCheck { get; set; }
        public bool BlackKingIsInCheck { get; set; }
        public bool PlayersAgreedToADraw { get; set; }
        public bool PlayerResigned { get; set; }
        public bool PlayerOfferedADraw { get; set; }

        public GameState(int gameId)
        {
            GameID = gameId;
            WhiteKingID = null;
            BlackKingID = null;
            WhitePawnThatCanBeTakenByEnPassantMoveID = null;
            BlackPawnThatCanBeTakenByEnPassantMoveID = null;
            CurrentPlayerPiecesAttackingTheKing = new List<Piece>();
            CurrentPlayer = Sides.White;
            WhiteKingIsInCheck = false;
            BlackKingIsInCheck = false;
            PlayersAgreedToADraw = false;
            PlayerResigned = false;
            PlayerOfferedADraw = false;
        }

        public GameState()
        {

        }

        public bool CurrentPlayerKingIsInCheck => CurrentPlayer == Sides.White ? WhiteKingIsInCheck : BlackKingIsInCheck;
        public bool IsACheckmate => CheckmateOccured();
        public bool IsAStalemate => StalemateOccured();
        public bool IsAWin => WinConditionMet();
        public bool IsADraw => DrawConditionMet();

        public bool DrawConditionMet()
        {
            /*   TODO someday:
             if (IsADeadPosition()) // automatic draw
             {
                 IsADraw = true;
             }
             else if (IsAFivefoldRepetition()) // automatic draw
             {
                 IsADraw = true;
             }
             else if (IsASeventyFiveMoveRule()) // automatic draw
             {
                 IsADraw = true;
             }
             else if (IsAFiftyMoveRule()) // not automatic draw
             {
                 IsADraw = true;
             }
             else if (IsAThreefoldRepetition()) // not automatic draw
             {
                 IsADraw = true;
             }*/

            return PlayersAgreedToADraw || IsAStalemate;
        }

        public bool StalemateOccured()
        {
            for (int i = 0; i < Board.boardSize; i++)
            {
                for (int j = 0; j < Board.boardSize; j++)
                {
                    int fieldAndPositionId = ((i * 8) + j + 1);
                    var piece = Game.Chessboard.BoardsFieldColumns.Single(s => s.GameID == GameID && s.FieldColumnID == i + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.FieldID == fieldAndPositionId).Content;
                    if (piece != null)
                    {
                        bool isOponentsPiece = CurrentPlayer == Sides.White ? !piece.IsWhite : piece.IsWhite;
                        if (isOponentsPiece && piece.NextAvailablePositions.Count != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void ChangeTurns()
        {
            CurrentPlayer = CurrentPlayer == Sides.White ? CurrentPlayer = Sides.Black
                                                         : CurrentPlayer = Sides.White;
        }

        public void ResetEnPassantFlag()
        {
            if (CurrentPlayer == Sides.White)
            {
                if (BlackPawnThatCanBeTakenByEnPassantMove != null)
                {
                    BlackPawnThatCanBeTakenByEnPassantMove.CanBeTakenByEnPassantMove = false;
                    BlackPawnThatCanBeTakenByEnPassantMove = null;
                }
            }
            else
            {
                if (WhitePawnThatCanBeTakenByEnPassantMove != null)
                {
                    WhitePawnThatCanBeTakenByEnPassantMove.CanBeTakenByEnPassantMove = false;
                    WhitePawnThatCanBeTakenByEnPassantMove = null;
                }
            }
        }

        public void ResetKingCheckFlag()
        {
            if (CurrentPlayer == Sides.White)
            {
                WhiteKingIsInCheck = false;
            }
            else
            {
                BlackKingIsInCheck = false;
            }
        }

        public bool CheckmateOccured()
        {
            bool oponentsKingIsInCheck = CurrentPlayer == Sides.White ? BlackKingIsInCheck : WhiteKingIsInCheck;
            if (oponentsKingIsInCheck)
            {
                bool oponentsKingIsDoubleChecked = CurrentPlayerPiecesAttackingTheKing.Count > 1;
                if (oponentsKingIsDoubleChecked && !KingCanMoveAway())
                {
                    return true;
                }
                else if (!KingCanMoveAway())
                {
                    if (CurrentPlayerPiecesAttackingTheKing[0].GetType() == typeof(IDiagonallyMovingPiece)
                        || CurrentPlayerPiecesAttackingTheKing[0].GetType() == typeof(IHorizontallyAndVerticallyMovingPiece))
                    {
                        if (!CheckCanBeBlockedOrAttackingPieceCanBeCaptured())
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (!AttackingPieceCanBeCaptured())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

            bool KingCanMoveAway()
            {
                int oponentsKingAvailablePositionsCount = CurrentPlayer == Sides.White ? BlackKing.NextAvailablePositions.Count
                                                                                       : WhiteKing.NextAvailablePositions.Count;
                return oponentsKingAvailablePositionsCount != 0;
            }

            bool AttackingPieceCanBeCaptured()
            {
                for (var i = 0; i < Board.boardSize; i++)
                {
                    for (var j = 0; j < Board.boardSize; j++)
                    {
                        int fieldAndPositionId = ((i * 8) + j + 1);
                        var piece = Game.Chessboard.BoardsFieldColumns.Single(s => s.GameID == GameID && s.FieldColumnID == i + 1)
                                        .FieldColumn.Fields.SingleOrDefault(s => s.FieldID == fieldAndPositionId).Content;
                        if (piece != null)
                        {
                            bool isOponentsPiece = CurrentPlayer == Sides.White ? !piece.IsWhite : piece.IsWhite;
                            if (isOponentsPiece)
                            {
                                if (piece.NextAvailablePositions.Contains(new NextAvailablePosition(CurrentPlayerPiecesAttackingTheKing[0].PieceID, CurrentPlayerPiecesAttackingTheKing[0].PositionID)))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }

            bool CheckCanBeBlockedOrAttackingPieceCanBeCaptured() // Return positions that block check
            {
                List<Position> blockingPositions = new();
                int currentPlayersPieceFile = Array.IndexOf(Board.files, CurrentPlayerPiecesAttackingTheKing[0].Position.FileID);
                int currentPlayersPieceRank = Array.IndexOf(Board.ranks, CurrentPlayerPiecesAttackingTheKing[0].Position.RankID);
                King oponentsKing = CurrentPlayer == Sides.White ? BlackKing : WhiteKing;
                int oponentsKingFile = Array.IndexOf(Board.files, oponentsKing.Position.FileID);
                int oponentsKingRank = Array.IndexOf(Board.ranks, oponentsKing.Position.RankID);

                int x = oponentsKingFile == currentPlayersPieceFile ? 0 : (oponentsKingFile > currentPlayersPieceFile ? 1 : -1);    // Check if both pieces are on the same file --> A rook or a queen is an attacking piece
                int y = oponentsKingRank == currentPlayersPieceRank ? 0 : (oponentsKingRank > currentPlayersPieceRank ? 1 : -1);    // Check if both pieces are on the same rank --> A rook or a queen is an attacking piece
                int file = currentPlayersPieceFile + x;                                                                             // Otherwise, pieces are on the different file and rank-- > A bishop or a queen is an attacking piece
                int rank = currentPlayersPieceRank + y;

                bool canMoveInBetween = y == 0 ? (oponentsKingFile > currentPlayersPieceFile ? file < oponentsKingFile : file > oponentsKingFile)
                                                : (oponentsKingRank > currentPlayersPieceRank ? rank < oponentsKingRank : rank > oponentsKingRank);
                int fieldAndPositionId = ((file * 8) + rank + 1);
                while (canMoveInBetween)
                {
                    
                    blockingPositions.Add(new Position(fieldAndPositionId, Board.files[file], Board.ranks[rank]));
                    file += x;
                    rank += y;
                }

                for (int i = 0; i < Board.boardSize; i++)
                {
                    for (int j = 0; j < Board.boardSize; j++)
                    {
                        var piece = Game.Chessboard.BoardsFieldColumns.Single(s => s.GameID == GameID && s.FieldColumnID == i + 1)
                                        .FieldColumn.Fields.SingleOrDefault(s => s.FieldID == fieldAndPositionId).Content;
                        if (piece != null)
                        {
                            bool isOponentsPiece = CurrentPlayer == Sides.White ? !piece.IsWhite : piece.IsWhite;
                            if (isOponentsPiece)
                            {
                                if (piece.NextAvailablePositions.Contains(new NextAvailablePosition(CurrentPlayerPiecesAttackingTheKing[0].PieceID, CurrentPlayerPiecesAttackingTheKing[0].PositionID)))    // Check if any of the oponents pieces can capture attacking piece
                                {
                                    return true;
                                }
                                foreach (Position position in blockingPositions)                                                 // Check if any of the oponents pieces can move on the blocking position
                                {
                                    foreach (NextAvailablePosition p in piece.NextAvailablePositions)
                                    {
                                        if ((p.Position.PositionID).Equals(position.PositionID))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }

        public void ResetCurrentPiecesAttackingTheKing()
        {
            CurrentPlayerPiecesAttackingTheKing.Clear();
        }

        public bool WinConditionMet()
        {
            return IsACheckmate || PlayerResigned;
        }
    }
}

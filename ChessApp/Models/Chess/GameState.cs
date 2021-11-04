using ChessApp.Models.Chess.Pieces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public List<Piece> CurrentPlayerPiecesAttackingTheKing { get; set; } = new List<Piece>();
        public enum Sides
        {
            White = 1,
            Black = 0
        }
        public Sides CurrentPlayer { get; set; } = Sides.White;
        public bool WhiteKingIsInCheck { get; set; } = false;
        public bool BlackKingIsInCheck { get; set; } = false;
        public bool CurrentPlayerKingIsInCheck => CurrentPlayer == Sides.White ? WhiteKingIsInCheck : BlackKingIsInCheck;
        public bool PlayersAgreedToADraw { get; set; } = false;
        public bool PlayerResigned { get; set; } = false;
        public bool PlayerOfferedADraw { get; set; } = false;
        public bool IsACheckmate => CheckmateOccured();
        public bool IsAStalemate => StalemateOccured();
        public bool IsAWin => WinConditionMet();
        public bool IsADraw => DrawConditionMet();

        public GameState(int gameId)
        {
            GameID = gameId;
        }

        public GameState()
        {

        }

        private bool DrawConditionMet()
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

        private bool StalemateOccured()
        {/*
            for (int i = 0; i < Board.boardSize; i++)
            {
                for (int j = 0; j < Board.boardSize; j++)
                {
                    Piece piece = board.FieldColumns[i].Fields[j].Content;
                    if (piece != null)
                    {
                        bool isOponentsPiece = CurrentPlayer == Sides.White ? !piece.IsWhite : piece.IsWhite;
                        if (isOponentsPiece && piece.NextAvailablePositions.Count != 0)
                        {
                            return false;
                        }
                    }
                }
            }*/
            return true;
        }

        private void ChangeTurns()
        {
            CurrentPlayer = CurrentPlayer == Sides.White ? CurrentPlayer = Sides.Black
                                                         : CurrentPlayer = Sides.White;
        }

        private void ResetEnPassantFlag()
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

        private bool CheckmateOccured()
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
            {/*
                for (var i = 0; i < Board.boardSize; i++)
                {
                    for (var j = 0; j < Board.boardSize; j++)
                    {
                        Piece piece = board.FieldColumns[i].Fields[j].Content;
                        if (piece != null)
                        {
                            bool isOponentsPiece = CurrentPlayer == Sides.White ? !piece.IsWhite : piece.IsWhite;
                            if (isOponentsPiece)
                            {
                                if (piece.NextAvailablePositions.Contains(CurrentPlayerPiecesAttackingTheKing[0].Position))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }*/
                return false;
            }

            bool CheckCanBeBlockedOrAttackingPieceCanBeCaptured() // Return positions that are blocking check
            {/*
                List<string> blockingPositions = new List<string>();//
                int currentPlayersPieceFile = board.BoardsFiles.IndexOf(board.BoardsFiles.Single(bf => bf.FileID == CurrentPlayerPiecesAttackingTheKing[0].Position.File.FileID));
                int currentPlayersPieceRank = board.BoardsRanks.IndexOf(board.BoardsRanks.Single(br => br.RankID == CurrentPlayerPiecesAttackingTheKing[0].Position.Rank.RankID));
                King oponentsKing = CurrentPlayer == Sides.White ? BlackKing : WhiteKing;
                int oponentsKingFile = board.BoardsFiles.IndexOf(board.BoardsFiles.Single(bf => bf.FileID == oponentsKing.Position.File.FileID));
                int oponentsKingRank = board.BoardsRanks.IndexOf(board.BoardsRanks.Single(br => br.RankID == oponentsKing.Position.Rank.RankID));

                int x = oponentsKingFile == currentPlayersPieceFile ? 0 : (oponentsKingFile > currentPlayersPieceFile ? 1 : -1);    // Check if both pieces are on the same file --> A rook or a queen is an attacking piece
                int y = oponentsKingRank == currentPlayersPieceRank ? 0 : (oponentsKingRank > currentPlayersPieceRank ? 1 : -1);    // Check if both pieces are on the same rank --> A rook or a queen is an attacking piece
                int file = currentPlayersPieceFile + x;                                                                             // Otherwise, pieces are on the different file and rank-- > A bishop or a queen is an attacking piece
                int rank = currentPlayersPieceRank + y;

                bool canMoveInBetween = y == 0 ? (oponentsKingFile > currentPlayersPieceFile ? file < oponentsKingFile : file > oponentsKingFile)
                                                : (oponentsKingRank > currentPlayersPieceRank ? rank < oponentsKingRank : rank > oponentsKingRank);
                while (canMoveInBetween)
                {
                    blockingPositions.Add(board.BoardsFiles[file].FileID + board.BoardsRanks[rank].RankID);
                    file += x;
                    rank += y;
                }

                for (int i = 0; i < Board.boardSize; i++)
                {
                    for (int j = 0; j < Board.boardSize; j++)
                    {
                        Piece piece = board.FieldColumns[i].Fields[j].Content;
                        if (piece != null)
                        {
                            bool isOponentsPiece = CurrentPlayer == Sides.White ? !piece.IsWhite : piece.IsWhite;
                            if (isOponentsPiece)
                            {
                                if (piece.NextAvailablePositions.Contains(CurrentPlayerPiecesAttackingTheKing[0].Position))    // Check if any of the oponents pieces can capture attacking piece
                                {
                                    return true;
                                }
                                foreach (string position in blockingPositions)                                                 // Check if any of the oponents pieces can move on the blocking position
                                {
                                    foreach (Position p in piece.NextAvailablePositions)
                                    {
                                        if ((p.Name).Equals(position))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }*/
                return false;
            }
        }

        private void ResetCurrentPiecesAttackingTheKing()
        {
            CurrentPlayerPiecesAttackingTheKing.Clear();
        }

        private bool WinConditionMet()
        {
            return IsACheckmate || PlayerResigned;
        }
    }
}

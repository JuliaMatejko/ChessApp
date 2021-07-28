using ChessApp.Models.Chess;
using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChessApp.Models.Chess
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public enum Sides
        {
            White = 1,
            Black = 0
        }
        [Required]
        public Sides CurrentPlayer { get; set; } = Sides.White;
        public Pawn WhitePawnThatCanBeTakenByEnPassantMove { get; set; }
        public Pawn BlackPawnThatCanBeTakenByEnPassantMove { get; set; }
        public King WhiteKing { get; set; }
        public King BlackKing { get; set; }
        [Required]
        public bool WhiteKingIsInCheck { get; set; } = false;
        [Required]
        public bool BlackKingIsInCheck { get; set; } = false;
        [Required]
        public bool CurrentPlayerKingIsInCheck => CurrentPlayer == Sides.White ? WhiteKingIsInCheck : BlackKingIsInCheck;
        public bool IsAWin => WinConditionMet();
        public bool IsADraw => DrawConditionMet();
        [Required]
        public bool PlayerResigned { get; set; } = false;
        [Required]
        public bool PlayerOfferedADraw { get; set; } = false;
        [Required]
        public bool PlayersAgreedToADraw { get; set; } = false;
        public bool IsAStalemate => StalemateOccured();
        public List<Piece> CurrentPlayerPiecesAttackingTheKing { get; set; } = new List<Piece>();
        public bool IsACheckmate => CheckmateOccured();
        public Board Board { get; } = new Board();
        [NotMapped]
        public Dictionary<string, Field> Fields
        {
            get => CreateFieldsDictionary(Board);
            set => _fields = value;
        }
        private Dictionary<string, Field> _fields;

        
        public void StartGame()
        {/*
            Console.WriteLine(" Let's play chess!");
            Console.WriteLine("");
            Console.WriteLine(" 1. To make a move, type a piece, current piece position and new piece position separated by a space, ex. 'pw e2 e4'");
            Console.WriteLine(" 2. To promote a pawn, type, ex. 'pw e7 e8 Q'");
            Console.WriteLine("    You can choose B|N|R|Q, where each letter corresponds to: B - bishop, N - knight, R - rook, Q - queen");
            Console.WriteLine(" 3. To castle, just move your king two squares and rook will go on the right place");
            Console.WriteLine(" 4. To resign, type 'resign'");
            Console.WriteLine(" 5. To propose a draw, type 'draw'");*/
            SetStartingBoard();
            //BoardController.RefreshAttackedSquares();
            //BoardView.PrintBoard(Board);
            /*
            while (!IsAWin && !IsADraw)
            {
                BoardController.MakeAMove();
                BoardController.RefreshAttackedSquares();
                if (PlayersAgreedToADraw)
                {
                    Console.WriteLine(" Players agreed to a draw.");
                    Console.WriteLine(" It's a draw!");
                }
                else if (PlayerResigned)
                {
                    Console.Write($" {CurrentPlayer} resigned.");
                    ChangeTurns();
                    Console.WriteLine($" {CurrentPlayer} won the game!");
                }
                else if (IsACheckmate)
                {
                    BoardView.PrintBoard(Board);
                    Console.WriteLine(" Checkmate.");
                    Console.WriteLine($" {CurrentPlayer} won the game!");
                }
                else if (IsAStalemate)
                {
                    BoardView.PrintBoard(Board);
                    Console.WriteLine(" Stalemate.");
                    Console.WriteLine(" It's a draw!");
                }
                else
                {
                    BoardView.PrintBoard(Board);
                    ResetEnPassantFlag();
                    ResetCurrentPiecesAttackingTheKing();
                    ChangeTurns();
                }
            }*/
        }
        
        public static Dictionary<string, Field> CreateFieldsDictionary(Board board)
        {
            Dictionary<string, Field> fields = new Dictionary<string, Field>();
            int i = 0;
            int j = 0;
            foreach (Position position in Board.Positions)
            {
                if (j < Board.boardSize)
                {
                    fields[position.Name] = board.FieldColumns[i].Fields[j];
                }
                else
                {
                    j = 0;
                    i++;
                    fields[position.Name] = board.FieldColumns[i].Fields[j];
                }
                j++;
            }
            return fields;
        }

        private void SetStartingBoard() // set pieces on the starting position on the board
        {
            for (var i = 0; i < Board.boardSize; i++)
            {
                Fields[Board.Files[i].FileID + "2"].Content = new Pawn(true, new Position(Board.Files[i].FileID, "2"));  // set white pawns
            }
            for (var i = 0; i < Board.boardSize; i++)
            {
                Fields[Board.Files[i].FileID + "7"].Content = new Pawn(false, new Position(Board.Files[i].FileID, "7"));  // set black pawns 
            }
            Fields["a1"].Content = new Rook(true, new Position("a", "1"));        // set white rooks
            Fields["h1"].Content = new Rook(true, new Position("h", "1"));
            Fields["a8"].Content = new Rook(false, new Position("a", "8"));       // set black rooks
            Fields["h8"].Content = new Rook(false, new Position("h", "8"));
            Fields["b1"].Content = new Knight(true, new Position("h", "8"));      // set white knights
            Fields["g1"].Content = new Knight(true, new Position("g", "1"));
            Fields["b8"].Content = new Knight(false, new Position("b", "8"));     // set black knights
            Fields["g8"].Content = new Knight(false, new Position("g", "8"));
            Fields["c1"].Content = new Bishop(true, new Position("c", "1"));      // set white bishops
            Fields["f1"].Content = new Bishop(true, new Position("f", "1"));
            Fields["c8"].Content = new Bishop(false, new Position("c", "8"));     // set black bishops
            Fields["f8"].Content = new Bishop(false, new Position("f", "8"));
            Fields["d1"].Content = new Queen(true, new Position("d", "1"));       // set white queen
            Fields["d8"].Content = new Queen(false, new Position("e", "1"));      // set black queen
            Fields["e1"].Content = new King(true, new Position("e", "1"));        // set white king
            WhiteKing = (King)Fields["e1"].Content;
            Fields["e8"].Content = new King(false, new Position("e", "8"));       // set black king
            BlackKing = (King)Fields["e8"].Content;
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

        private void ResetCurrentPiecesAttackingTheKing()
        {
            CurrentPlayerPiecesAttackingTheKing.Clear();
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
        
        private bool WinConditionMet()
        {
            return IsACheckmate || PlayerResigned;
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
            {
                for (var i = 0; i < Board.boardSize; i++)
                {
                    for (var j = 0; j < Board.boardSize; j++)
                    {
                        Piece piece = Board.FieldColumns[i].Fields[j].Content;
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
                }
                return false;
            }

            bool CheckCanBeBlockedOrAttackingPieceCanBeCaptured() // Return positions that are blocking check
            {
                List<string> blockingPositions = new List<string>();
                int currentPlayersPieceFile = Board.Files.IndexOf(CurrentPlayerPiecesAttackingTheKing[0].Position.File);
                int currentPlayersPieceRank = Board.Ranks.IndexOf(CurrentPlayerPiecesAttackingTheKing[0].Position.Rank);
                King oponentsKing = CurrentPlayer == Sides.White ? BlackKing : WhiteKing;
                int oponentsKingFile = Board.Files.IndexOf(oponentsKing.Position.File);
                int oponentsKingRank = Board.Ranks.IndexOf(oponentsKing.Position.Rank);

                int x = oponentsKingFile == currentPlayersPieceFile ? 0 : (oponentsKingFile > currentPlayersPieceFile ? 1 : -1);    // Check if both pieces are on the same file --> A rook or a queen is an attacking piece
                int y = oponentsKingRank == currentPlayersPieceRank ? 0 : (oponentsKingRank > currentPlayersPieceRank ? 1 : -1);    // Check if both pieces are on the same rank --> A rook or a queen is an attacking piece
                int file = currentPlayersPieceFile + x;                                                                             // Otherwise, pieces are on the different file and rank-- > A bishop or a queen is an attacking piece
                int rank = currentPlayersPieceRank + y;

                bool canMoveInBetween = y == 0 ? (oponentsKingFile > currentPlayersPieceFile ? file < oponentsKingFile : file > oponentsKingFile)
                                                : (oponentsKingRank > currentPlayersPieceRank ? rank < oponentsKingRank : rank > oponentsKingRank);
                while (canMoveInBetween)
                {
                    blockingPositions.Add(Board.Files[file].FileID + Board.Ranks[rank].RankID);
                    file += x;
                    rank += y;
                }

                for (int i = 0; i < Board.boardSize; i++)
                {
                    for (int j = 0; j < Board.boardSize; j++)
                    {
                        Piece piece = Board.FieldColumns[i].Fields[j].Content;
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
                }
                return false;
            }
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
        {
            for (int i = 0; i < Board.boardSize; i++)
            {
                for (int j = 0; j < Board.boardSize; j++)
                {
                    Piece piece = Board.FieldColumns[i].Fields[j].Content;
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
    }
}

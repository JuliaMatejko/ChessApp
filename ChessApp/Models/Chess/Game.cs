using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static ChessApp.Models.Chess.GameState;

namespace ChessApp.Models.Chess
{
    public class Game
    {
        private enum PieceId
        {
            pw1 = 1, pw2, pw3, pw4, pw5, pw6, pw7, pw8,
            pb1, pb2, pb3, pb4, pb5, pb6, pb7, pb8,
            rw1, rw2, rb1, rb2,
            nw1, nw2, nb1, nb2,
            bw1, bw2, bb1, bb2,
            qw, qb,
            kw, kb
        }

        private enum FileIndex
        {
            a, b, c, d, e, f, g, h
        }

        private enum RankIndex
        {
            r1, r2, r3, r4, r5, r6, r7, r8
        }

        public int GameID { get; set; }
        [Display(Name = "First Player")]
        public string FirstPlayerID { get; set; }
        [Display(Name = "Second Player")]
        public string SecondPlayerID { get; set; }
        public Board Chessboard { get; set; }
        public GameState GameState { get; set; } 
        public List<Move> Moves { get; set; }
        public static readonly string[] pieceNames = { "qb", "qw", "nw", "nb", "rw", "rb", "bw", "bb", "kw","kb","pw","pb" };

        public Game(File[] files, Rank[] ranks, Position[] positions, Field[] fields, FieldColumn[] fieldColumns)
        {
            Chessboard = new(GameID, files, ranks, positions, fields, fieldColumns);
            Moves = new();
            GameState = new(this);
        }

        public Game()
        {

        }

        public void StartGame()
        {
            SetStartingBoard();

            RefreshAttackedSquares();

            BoardView.PrintBoard(Chessboard);//d

            while (!GameState.IsAWin && !GameState.IsADraw)
            {
                MakeAMove();
                RefreshAttackedSquares();
                if (GameState.PlayersAgreedToADraw)
                {
                    Console.WriteLine(" Players agreed to a draw.");//d
                    Console.WriteLine(" It's a draw!");//d
                }
                else if (GameState.PlayerResigned)
                {
                    Console.Write($" {GameState.CurrentPlayer} resigned.");//d
                    GameState.ChangeTurns();
                    Console.WriteLine($" {GameState.CurrentPlayer} won the game!");//d
                }
                else if (GameState.IsACheckmate)
                {
                    BoardView.PrintBoard(Chessboard);//d

                    Console.WriteLine(" Checkmate.");//d
                    Console.WriteLine($" {GameState.CurrentPlayer} won the game!");//d
                }
                else if (GameState.IsAStalemate)
                {
                    BoardView.PrintBoard(Chessboard);//d

                    Console.WriteLine(" Stalemate.");//d
                    Console.WriteLine(" It's a draw!");//d
                }
                else
                {
                    BoardView.PrintBoard(Chessboard);//d
                    GameState.ResetEnPassantFlag();
                    GameState.ResetCurrentPiecesAttackingTheKing();
                    GameState.ChangeTurns();
                }
            }
        }

        //TO DO someday: 1. SetBoard() -from standard board notation 2. Do an analysis board - without time, ability to undo a move

        /* set pieces on the starting position on the board */
        public void SetStartingBoard()
        {
            // set white pawns
            Chessboard.BoardsFieldColumns[(int)FileIndex.a].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw1, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.b].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw2, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.c].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw3, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.d].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw4, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.e].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw5, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.f].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw6, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.g].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw7, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.h].FieldColumn.Fields[(int)RankIndex.r2].Content
                = new Pawn(GameID, (int)PieceId.pw8, true, Chessboard.BoardsPositions[(int)RankIndex.r2].Position, GameState);
            // set black pawns 
            Chessboard.BoardsFieldColumns[(int)FileIndex.a].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb1, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.b].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb2, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.c].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb3, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.d].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb4, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.e].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb5, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.f].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb6, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.g].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb7, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.h].FieldColumn.Fields[(int)RankIndex.r7].Content
                = new Pawn(GameID, (int)PieceId.pb8, false, Chessboard.BoardsPositions[(int)RankIndex.r7].Position, GameState);
            // set white rooks
            Chessboard.BoardsFieldColumns[(int)FileIndex.a].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new Rook(GameID, (int)PieceId.rw1, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.h].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new Rook(GameID, (int)PieceId.rw2, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            // set black rooks
            Chessboard.BoardsFieldColumns[(int)FileIndex.a].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new Rook(GameID, (int)PieceId.rb1, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.h].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new Rook(GameID, (int)PieceId.rb2, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            // set white knights
            Chessboard.BoardsFieldColumns[(int)FileIndex.b].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new Knight(GameID, (int)PieceId.nw1, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.g].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new Knight(GameID, (int)PieceId.nw2, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            // set black knights
            Chessboard.BoardsFieldColumns[(int)FileIndex.b].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new Knight(GameID, (int)PieceId.nb1, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.g].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new Knight(GameID, (int)PieceId.nb2, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            // set white bishops
            Chessboard.BoardsFieldColumns[(int)FileIndex.c].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new Bishop(GameID, (int)PieceId.bw1, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.f].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new Bishop(GameID, (int)PieceId.bw2, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            // set black bishops
            Chessboard.BoardsFieldColumns[(int)FileIndex.c].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new Bishop(GameID, (int)PieceId.bb1, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            Chessboard.BoardsFieldColumns[(int)FileIndex.f].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new Bishop(GameID, (int)PieceId.bb2, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            // set white queen
            Chessboard.BoardsFieldColumns[(int)FileIndex.d].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new Queen(GameID, (int)PieceId.qw, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            // set black queen
            Chessboard.BoardsFieldColumns[(int)FileIndex.d].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new Queen(GameID, (int)PieceId.qb, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            // set white king
            Chessboard.BoardsFieldColumns[(int)FileIndex.e].FieldColumn.Fields[(int)RankIndex.r1].Content
                = new King(GameID, (int)PieceId.kw, true, Chessboard.BoardsPositions[(int)RankIndex.r1].Position, GameState);
            GameState.WhiteKing
                = (King)Chessboard.BoardsFieldColumns[(int)FileIndex.e].FieldColumn.Fields[(int)RankIndex.r1].Content;
            // set black king
            Chessboard.BoardsFieldColumns[(int)FileIndex.e].FieldColumn.Fields[(int)RankIndex.r8].Content
                = new King(GameID, (int)PieceId.kb, false, Chessboard.BoardsPositions[(int)RankIndex.r8].Position, GameState);
            GameState.BlackKing
                = (King)Chessboard.BoardsFieldColumns[(int)FileIndex.e].FieldColumn.Fields[(int)RankIndex.r8].Content;
        }

        public void RefreshAttackedSquares()
        {
            for (var i = 0; i < Board.boardSize; i++)
            {
                for (var j = 0; j < Board.boardSize; j++)
                {
                    int fieldAndPositionId = ((i * 8) + j + 1);
                    var piece = Chessboard.BoardsFieldColumns.Single(s => s.GameID == GameID && s.FieldColumnID == i + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldAndPositionId).Content;
                    if (piece != null && piece.GetType() != typeof(King))
                    {
                        piece.ControlledSquares.Clear();
                        piece.NextAvailablePositions = piece.ReturnAvailablePieceMoves(Chessboard);
                    }
                }
            }
            GameState.WhiteKing.ControlledSquares.Clear();
            GameState.WhiteKing.NextAvailablePositions = GameState.WhiteKing.ReturnAvailablePieceMoves(Chessboard);
            GameState.BlackKing.ControlledSquares.Clear();
            GameState.BlackKing.NextAvailablePositions = GameState.BlackKing.ReturnAvailablePieceMoves(Chessboard);
        }

        public void MakeAMove()
        {
            string chosenMove = null;
            Move move = null;
            Piece piece = null;
            Piece pieceCloned = null;
            Piece newPositionContentCloned = null;

            GetInputAndValidateIt(ref chosenMove, ref move);
            if (!GameState.PlayerResigned && !GameState.PlayerOfferedADraw)
            {
                FindPieceAndValidateMove(ref chosenMove, ref move, ref piece);
                ClonePieceAndNewPositionPiece(move.NewPosition, piece, ref pieceCloned, ref newPositionContentCloned);
                ChangeBoard(move, piece);
                GameState.ResetKingCheckFlag();
                RefreshAttackedSquares();

                while (GameState.CurrentPlayerKingIsInCheck)
                {
                    UndoBoardChanges(move, pieceCloned, newPositionContentCloned);
                    GameState.ResetKingCheckFlag();
                    RefreshAttackedSquares();
                    Console.WriteLine($" It's not a valid move. Your king would be in check. Choose a differnt piece or/and field.");
                    MakeAMove();
                }
                Moves.Add(move);
            }
            else if (GameState.PlayerOfferedADraw)
            {
                if (OponentAcceptsADraw())
                {
                    GameState.PlayersAgreedToADraw = true;
                }
                else
                {
                    GameState.PlayerOfferedADraw = false;
                    Console.WriteLine(" Draw denied. Make a move or resign");
                    MakeAMove();
                }
            }
        }

        //    TO DO : Rewrite to javascript client side validation!!   use javascript and frontend to get the input and change parameters values in this method
        public void GetInputAndValidateIt(ref string chosenMove, ref Move move)
        {
            Console.Write($" {GameState.CurrentPlayer} turn, make a move: ");  // TO DO: frontend action
            chosenMove = Console.ReadLine();
            while (!UserInputIsValid(chosenMove, ref move))
            {
                Console.Write($" It's not a valid input. Type your move in a correct format: ");     // TO DO: frontend action
                chosenMove = Console.ReadLine();
            }
        }

        public bool UserInputIsValid(string chosenMove, ref Move move) //TO DO: rewrite using regex
        {
            if (chosenMove.Length == 8 
                && chosenMove[2] == ' '
                && chosenMove[5] == ' ' 
                && Board.files.Contains(chosenMove[3].ToString())
                && Board.ranks.Contains(chosenMove[4].ToString())
                && Board.files.Contains(chosenMove[6].ToString())
                && Board.ranks.Contains(chosenMove[7].ToString()))
            {
                move = StringToMove(chosenMove);
                int currentPositionId = (int)move.CurrentPositionID;
                int newPositionId = move.NewPositionID;
                bool x = Chessboard.BoardsPositions.SingleOrDefault(x => x.PositionID == currentPositionId) != null;
                bool y = Chessboard.BoardsPositions.SingleOrDefault(x => x.PositionID == newPositionId) != null;
                if (pieceNames.Contains(move.PieceNameID) && x && y
                    && !((move.PieceNameID == "pw" && move.CurrentPosition.RankID == "7")
                          || (move.PieceNameID == "pb" && move.CurrentPosition.RankID == "2")))
                {
                    return true;
                }
            }
            else if (chosenMove.Length == 11 
                && chosenMove[2] == ' ' 
                && chosenMove[5] == ' ' 
                && chosenMove[8] == ' '
                && Board.files.Contains(chosenMove[3].ToString())
                && Board.ranks.Contains(chosenMove[4].ToString())
                && Board.files.Contains(chosenMove[6].ToString())
                && Board.ranks.Contains(chosenMove[7].ToString()))
            {
                move = StringToMove(chosenMove);
                int currentPositionId = (int)move.CurrentPositionID;
                int newPositionId = move.NewPositionID;
                bool x = Chessboard.BoardsPositions.SingleOrDefault(x => x.PositionID == currentPositionId) != null;
                bool y = Chessboard.BoardsPositions.SingleOrDefault(x => x.PositionID == newPositionId) != null;
                string[] piecesToPromote = { "qb","qw", "nw", "nb","rw","rb", "bw", "bb" };
                if (pieceNames.Contains(move.PieceNameID) && x && y && piecesToPromote.Contains(move.PromotionToID))
                {
                    return true;
                }
            }
            else if (chosenMove == "resign")
            {
                GameState.PlayerResigned = true;
                return true;
            }
            else if (chosenMove == "draw")
            {
                GameState.PlayerOfferedADraw = true;
                return true;
            }
            return false;
        }

        public Move StringToMove(string str) // str parameter format ex.: "pw e2 e4" or "pw e7 e8 pw" (if its pawn promotion)
        {
            string[] substrings = str.Split(" ");
            Position currentPosition = Chessboard.BoardsPositions.Single(s => s.Position.Name == substrings[1]).Position;
            Position nextPosition = Chessboard.BoardsPositions.Single(s => s.Position.Name == substrings[2]).Position;
            Move move;
            if (str.Length == 8)
            {
                move = new(GameID, substrings[0], currentPosition, nextPosition);
            }
            else
            {
                move = new Move(GameID, substrings[0], currentPosition, nextPosition, substrings[3]);
            }
            return move;
        }

        public void FindPieceAndValidateMove(ref string chosenMove, ref Move move, ref Piece piece)
        {
            piece = FindPiece(move.PieceNameID, move.CurrentPosition);
            while (!MoveIsPossible(piece, move))
            {
                Console.Write($" It's not a valid move. Choose a differnt piece or/and field: "); // TO DO: przepisz pod javascript (migające na czerwono pole, gdy MoveIsPossible = false
                chosenMove = Console.ReadLine();
                move = StringToMove(chosenMove);
                piece = FindPiece(move.PieceNameID, move.CurrentPosition);
            }

            bool MoveIsPossible(Piece piece, Move move)
            {
                if (piece != null && ChosenPieceIsCurrentPlayersPiece(piece))
                {
                    return MoveIsCorrectPieceMove(piece, move);
                }
                return false;


                bool MoveIsCorrectPieceMove(Piece piece, Move move)
                {
                    bool x = piece.NextAvailablePositions.SingleOrDefault(x => x.PieceID.Equals(piece.PieceID) && x.PositionID.Equals(move.NewPositionID)) != null;
                    return x;
                }

                bool ChosenPieceIsCurrentPlayersPiece(Piece piece)
                {
                    return (piece.IsWhite && GameState.CurrentPlayer == GameState.Sides.White)
                            || (!piece.IsWhite && GameState.CurrentPlayer == GameState.Sides.Black);
                }
            }
        }

        public Piece FindPiece(string pieceName, Position currentPosition)
        {
            int indexFile = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == currentPosition.FileID));
            int indexRank = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == currentPosition.RankID));
            Piece piece = Chessboard.BoardsFieldColumns[indexFile].FieldColumn.Fields[indexRank].Content;
            if (piece == null)
            {
                return null;
            }
            else if (piece.PieceNameID == pieceName)
            {
                return piece;
            }
            return null;
        }

        public void ClonePieceAndNewPositionPiece(Position newPosition, Piece piece, ref Piece clonedPiece, ref Piece clonedNewPositionContent)//newPosition.File
        {
            int indexFile = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == newPosition.FileID));
            int indexRank = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == newPosition.RankID));
            clonedNewPositionContent = Chessboard.BoardsFieldColumns[indexFile].FieldColumn.Fields[indexRank].Content;
            if (clonedNewPositionContent != null)
            {
                clonedNewPositionContent = (Piece)clonedNewPositionContent.Clone();
            }
            else
            {
                clonedNewPositionContent = null;
            }
            clonedPiece = (Piece)piece.Clone();
        }

        public void ChangeBoard(Move move, Piece piece)
        {
            if (piece.GetType() == typeof(Pawn))
            {
                Pawn pawn = (Pawn)piece;
                if (pawn.IsFirstMove && Math.Abs(Convert.ToInt32(move.NewPosition.RankID) - Convert.ToInt32(move.CurrentPosition.RankID)) == 2)   // pawn moved two squares
                {
                    pawn.IsFirstMove = false;   // pawn cannot move two squares anymore
                    pawn.CanBeTakenByEnPassantMove = true;   // pawn might be taken en passant in a next move if other necessary conditions will occur
                    if (pawn.IsWhite)
                    {
                        GameState.WhitePawnThatCanBeTakenByEnPassantMove = pawn;
                    }
                    else
                    {
                        GameState.BlackPawnThatCanBeTakenByEnPassantMove = pawn;
                    }
                }
                else if (pawn.IsFirstMove)
                {
                    pawn.IsFirstMove = false;
                }
                else if ((pawn.IsWhite && pawn.Position.RankID == "7") || (!pawn.IsWhite && pawn.Position.RankID == "2"))   // promote pawn
                {
                    PawnPromotion(move, pawn.PieceID, pawn.IsWhite);
                }
                else if ((pawn.IsWhite && pawn.Position.RankID == "5" && move.NewPosition.FileID != pawn.Position.FileID)
                         || (!pawn.IsWhite && pawn.Position.RankID == "4" && move.NewPosition.FileID != pawn.Position.FileID))   // en passant capture
                {
                    int indexFile = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == move.NewPosition.FileID));
                    int indexRank = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == move.CurrentPosition.RankID));
                    Chessboard.BoardsFieldColumns[indexFile].FieldColumn.Fields[indexRank].Content = null;
                }
            }
            else if (piece.GetType() == typeof(King))
            {
                King king = (King)piece;
                if (king.IsFirstMove)
                {
                    king.IsFirstMove = false;   // king cannot castle anymore
                }
                int squaresMoved = Array.IndexOf(Board.files, move.NewPosition.FileID)
                                   - Array.IndexOf(Board.files, move.CurrentPosition.FileID);
                if (Math.Abs(squaresMoved) == 2)   // king castled
                {
                    Position oldPosition = new();
                    Position newPosition = new();
                    Rook rook;
                    if (squaresMoved == 2)   // king castled king side
                    {
                        oldPosition.FileID = "h";
                        newPosition.FileID = "f";
                    }
                    else   // squaresMoved == -2, king castled queen side
                    {
                        oldPosition.FileID = "a";
                        newPosition.FileID = "d";
                    }
                    oldPosition.RankID = move.NewPosition.RankID;
                    newPosition.RankID = move.NewPosition.RankID;
                    int indexFileOld = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == oldPosition.FileID));
                    int indexRankOld = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == oldPosition.RankID));
                    rook = (Rook)Chessboard.BoardsFieldColumns[indexFileOld].FieldColumn.Fields[indexRankOld].Content; // move a rook
                    rook.Position = newPosition;
                    int indexFile = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == newPosition.FileID));
                    int indexRank = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == newPosition.RankID));
                    Chessboard.BoardsFieldColumns[indexFile].FieldColumn.Fields[indexRank].Content = rook;
                    Chessboard.BoardsFieldColumns[indexFileOld].FieldColumn.Fields[indexRankOld].Content = null;
                }
            }
            else if (piece.GetType() == typeof(Rook))
            {
                Rook rook = (Rook)piece;
                if (rook.IsFirstMove)
                {
                    rook.IsFirstMove = false;   // rook cannot castle anymore
                }
            }
            piece.Position = move.NewPosition;
            int indexFileNew= Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == move.NewPosition.FileID));
            int indexRankNew = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == move.NewPosition.RankID));
            int indexFileCurrent = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == move.CurrentPosition.FileID));
            int indexRankCurrent = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == move.CurrentPosition.RankID));
            Chessboard.BoardsFieldColumns[indexFileNew].FieldColumn.Fields[indexRankNew].Content = Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn
                                                                                                             .Fields[indexRankCurrent].Content;   // move piece on a new position
            Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content = null;
        }

        public bool OponentAcceptsADraw()      // TO DO: frontend action
        {
            Console.Write($" {GameState.CurrentPlayer} offers a draw. Accept a draw? [yes|no]: ");  // TO DO: frontend action
            string acceptADraw = Console.ReadLine();//
            while (!(acceptADraw == "yes" || acceptADraw == "no"))
            {
                Console.Write($"Wrong response. Accept a draw? Type 'yes' to agree to draw or 'no' to continue the game: ");//
                acceptADraw = Console.ReadLine();//
            }
            return acceptADraw == "yes";
        }

        public void UndoBoardChanges(Move move, Piece pieceCloned, Piece newPositionContentCloned)
        {
            int indexFileNew = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == move.NewPosition.FileID));
            int indexRankNew = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == move.NewPosition.RankID));
            int indexFileCurrent = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == move.CurrentPosition.FileID));
            int indexRankCurrent = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == move.CurrentPosition.RankID));
            Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content = pieceCloned;   // move piece on a previous position
            Chessboard.BoardsFieldColumns[indexFileNew].FieldColumn.Fields[indexRankNew].Content = newPositionContentCloned;   // restore a previous NewPosition content
        }

        public void PawnPromotion(Move move, int pieceId, bool isWhite)
        {
            int indexFileCurrent = Chessboard.BoardsFiles.IndexOf(Chessboard.BoardsFiles.Find(s => s.GameID == GameID && s.FileID == move.CurrentPosition.FileID));
            int indexRankCurrent = Chessboard.BoardsRanks.IndexOf(Chessboard.BoardsRanks.Find(s => s.GameID == GameID && s.RankID == move.CurrentPosition.RankID));
            switch (move.PromotionTo.PieceNameID)
            {
                case "qw":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content 
                              = new Queen(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "qb":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content 
                              = new Queen(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "nw":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content
                              = new Knight(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "nb":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content
                               = new Knight(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "rw":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content
                              = new Rook(GameID, pieceId, isWhite, move.NewPosition, false, GameState);
                    break;
                case "rb":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content
                              = new Rook(GameID, pieceId, isWhite, move.NewPosition, false, GameState);
                    break;
                case "bw":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content
                               = new Bishop(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "bb":
                    Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content
                              = new Bishop(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
            }
            Chessboard.BoardsFieldColumns[indexFileCurrent].FieldColumn.Fields[indexRankCurrent].Content = null;
        }
    }
}

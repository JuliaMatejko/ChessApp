using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChessApp.Models.Chess
{
    public class Game
    {
        private static int _nextGameId = 0;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GameID { get; set; }

        [Display(Name = "White player")]
        public string WhitePlayerID { get; set; }
        [Display(Name = "Black player")]
        public string BlackPlayerID { get; set; }
        public Board Chessboard { get; set; }
        public GameState GameState { get; set; } 
        public List<Move> Moves { get; set; }//TO DO: dodaj ruch do listy po każdym ruchu
        public static readonly string[] pieceNames = { "qb", "qw", "nw", "nb", "rw", "rb", "bw", "bb", "kw","kb","pw","pb" };

        public Game(File[] files, Rank[] ranks, Position[] positions, FieldColumn[] fieldColumns)
        {
            _nextGameId += 1;
            GameID = _nextGameId;
            Chessboard = new(GameID, files, ranks, positions, fieldColumns);
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

        public Dictionary<string, Field> CreateFieldsDictionary()
        {
            Dictionary<string, Field> fields = new();
            int i = 0;
            int j = 0;
            int fieldAndPositionId;
            foreach (BoardPosition boardPosition in Chessboard.BoardsPositions.Where(s => s.GameID == Chessboard.GameID))
            {
                if (j < Board.boardSize)
                {
                    fieldAndPositionId = (i * 8) + j + 1;
                    fields[boardPosition.Position.Name] = Chessboard.BoardsFieldColumns.Single(s => s.GameID == Chessboard.GameID && s.FieldColumnID == i + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldAndPositionId);
                }
                else
                {
                    j = 0;
                    i++;
                    fieldAndPositionId = (i * 8) + j + 1;
                    fields[boardPosition.Position.Name] = Chessboard.BoardsFieldColumns.Single(s => s.GameID == Chessboard.GameID && s.FieldColumnID == i + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldAndPositionId);
                }
                j++;
                
            }
            return fields;
        }

        //TO DO someday: 1. SetBoard() -from standard board notation 2. Do an analysis board - without time, ability to undo a move

        public void SetStartingBoard() // set pieces on the starting position on the board
        {
            for (var i = 0; i < Board.boardSize; i++)
            {
                Chessboard.BoardsFieldColumns[i].FieldColumn.Fields[1].Content = new Pawn(GameID, i + 1, true, new Position((i * 8) + 2, Chessboard.BoardsFiles[i].File, Chessboard.BoardsRanks[1].Rank), GameState);  // set white pawns
            }
            for (var i = 0; i < Board.boardSize; i++)
            {
                Chessboard.BoardsFieldColumns[i].FieldColumn.Fields[6].Content = new Pawn(GameID, i + 9, false, new Position((i * 8) + 7, Chessboard.BoardsFiles[i].File, Chessboard.BoardsRanks[6].Rank), GameState);  // set black pawns 
            }
            Chessboard.BoardsFieldColumns[0].FieldColumn.Fields[0].Content = new Rook(GameID, 17, true, new Position(1, Chessboard.BoardsFiles[0].File, Chessboard.BoardsRanks[0].Rank), GameState);        // set white rooks
            Chessboard.BoardsFieldColumns[7].FieldColumn.Fields[0].Content = new Rook(GameID, 18, true, new Position(57, Chessboard.BoardsFiles[7].File, Chessboard.BoardsRanks[0].Rank), GameState);
            Chessboard.BoardsFieldColumns[0].FieldColumn.Fields[7].Content = new Rook(GameID, 19, false, new Position(8, Chessboard.BoardsFiles[0].File, Chessboard.BoardsRanks[7].Rank), GameState);       // set black rooks
            Chessboard.BoardsFieldColumns[7].FieldColumn.Fields[7].Content  = new Rook(GameID, 20, false, new Position(64, Chessboard.BoardsFiles[7].File, Chessboard.BoardsRanks[7].Rank), GameState);
            Chessboard.BoardsFieldColumns[1].FieldColumn.Fields[0].Content = new Knight(GameID, 21, true, new Position(9, Chessboard.BoardsFiles[1].File, Chessboard.BoardsRanks[0].Rank), GameState);      // set white knights
            Chessboard.BoardsFieldColumns[6].FieldColumn.Fields[0].Content = new Knight(GameID, 22, true, new Position(49, Chessboard.BoardsFiles[6].File, Chessboard.BoardsRanks[0].Rank), GameState);
            Chessboard.BoardsFieldColumns[1].FieldColumn.Fields[7].Content = new Knight(GameID, 23, false, new Position(16, Chessboard.BoardsFiles[1].File, Chessboard.BoardsRanks[7].Rank), GameState);     // set black knights
            Chessboard.BoardsFieldColumns[6].FieldColumn.Fields[7].Content = new Knight(GameID, 24, false, new Position(56, Chessboard.BoardsFiles[6].File, Chessboard.BoardsRanks[7].Rank), GameState);
            Chessboard.BoardsFieldColumns[2].FieldColumn.Fields[0].Content = new Bishop(GameID, 25, true, new Position(17, Chessboard.BoardsFiles[2].File, Chessboard.BoardsRanks[0].Rank), GameState);      // set white bishops
            Chessboard.BoardsFieldColumns[5].FieldColumn.Fields[0].Content = new Bishop(GameID, 26, true, new Position(41, Chessboard.BoardsFiles[5].File, Chessboard.BoardsRanks[0].Rank), GameState);
            Chessboard.BoardsFieldColumns[2].FieldColumn.Fields[7].Content = new Bishop(GameID, 27, false, new Position(24, Chessboard.BoardsFiles[2].File, Chessboard.BoardsRanks[7].Rank), GameState);     // set black bishops
            Chessboard.BoardsFieldColumns[5].FieldColumn.Fields[7].Content = new Bishop(GameID, 28, false, new Position(48, Chessboard.BoardsFiles[5].File, Chessboard.BoardsRanks[7].Rank), GameState);
            Chessboard.BoardsFieldColumns[3].FieldColumn.Fields[0].Content = new Queen(GameID, 29, true, new Position(25, Chessboard.BoardsFiles[3].File, Chessboard.BoardsRanks[0].Rank), GameState);       // set white queen
            Chessboard.BoardsFieldColumns[3].FieldColumn.Fields[7].Content = new Queen(GameID, 30, false, new Position(32, Chessboard.BoardsFiles[3].File, Chessboard.BoardsRanks[7].Rank), GameState);      // set black queen
            Chessboard.BoardsFieldColumns[4].FieldColumn.Fields[0].Content = new King(GameID, 31, true, new Position(33, Chessboard.BoardsFiles[4].File, Chessboard.BoardsRanks[0].Rank), GameState);        // set white king
            GameState.WhiteKing = (King)Chessboard.BoardsFieldColumns[4].FieldColumn.Fields[0].Content;
            Chessboard.BoardsFieldColumns[4].FieldColumn.Fields[7].Content = new King(GameID, 32, false, new Position(40, Chessboard.BoardsFiles[4].File, Chessboard.BoardsRanks[7].Rank), GameState);       // set black king
            GameState.BlackKing = (King)Chessboard.BoardsFieldColumns[4].FieldColumn.Fields[7].Content;
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

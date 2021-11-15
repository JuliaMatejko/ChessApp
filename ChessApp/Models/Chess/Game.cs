using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChessApp.Models.Chess
{
    public class Game
    {
        public int GameID { get; set; }
 
        public Board Chessboard { get; set; }
        public GameState GameState { get; set; } 
        public List<Move> Moves { get; set; }//TO DO: dodaj ruch do listy po każdym ruchu
        public static readonly string[] pieceNames = { "qb", "qw", "nw", "nb", "rw", "rb", "bw", "bb", "kw","kb","pw","pb" };

        public Game()
        {
            Chessboard = new(GameID);
            GameState = new(GameID);
        }

        [NotMapped]
        public Dictionary<string, Field> Fields
        {
            get => CreateFieldsDictionary();
            set => _fields = value;
        }
        private Dictionary<string, Field> _fields;

        public void StartGame()
        {
            Console.WriteLine(" Let's play chess!");
            Console.WriteLine("");
            Console.WriteLine(" 1. To make a move, type a piece, current piece position and new piece position separated by a space, ex. 'pw e2 e4'");
            Console.WriteLine(" 2. To promote a pawn, type, ex. 'pw e7 e8 Q'");
            Console.WriteLine("    You can choose qb,qw, nw, nb,rw,rb,bw,bb, where each first letter corresponds to piece name and second letter to color: b - bishop, n - knight, r - rook, q - queen so ex. qw is white queen");
            Console.WriteLine(" 3. To castle, just move your king two squares and rook will go on the right place");
            Console.WriteLine(" 4. To resign, type 'resign'");
            Console.WriteLine(" 5. To propose a draw, type 'draw'");//d

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

        private Dictionary<string, Field> CreateFieldsDictionary()
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

        private void SetStartingBoard() // set pieces on the starting position on the board
        {
            for (var i = 0; i < Board.boardSize; i++)
            {
                Fields[Chessboard.BoardsFiles[i].FileID + "2"].Content = new Pawn(GameID, i + 1, true, new Position((i * 8) + 2, Chessboard.BoardsFiles[i].FileID, "2"), GameState);  // set white pawns
            }
            for (var i = 0; i < Board.boardSize; i++)
            {
                Fields[Chessboard.BoardsFiles[i].FileID + "7"].Content = new Pawn(GameID, i + 9, false, new Position((i * 8) + 7, Chessboard.BoardsFiles[i].FileID, "7"), GameState);  // set black pawns 
            }
            Fields["a1"].Content = new Rook(GameID, 17, true, new Position(1, "a", "1"), GameState);        // set white rooks
            Fields["h1"].Content = new Rook(GameID, 18, true, new Position(57, "h", "1"), GameState);
            Fields["a8"].Content = new Rook(GameID, 19, false, new Position(8, "a", "8"), GameState);       // set black rooks
            Fields["h8"].Content = new Rook(GameID, 20, false, new Position(64, "h", "8"), GameState);
            Fields["b1"].Content = new Knight(GameID, 21, true, new Position(9, "b", "1"), GameState);      // set white knights
            Fields["g1"].Content = new Knight(GameID, 22, true, new Position(49, "g", "1"), GameState);
            Fields["b8"].Content = new Knight(GameID, 23, false, new Position(16, "b", "8"), GameState);     // set black knights
            Fields["g8"].Content = new Knight(GameID, 24, false, new Position(56, "g", "8"), GameState);
            Fields["c1"].Content = new Bishop(GameID, 25, true, new Position(17, "c", "1"), GameState);      // set white bishops
            Fields["f1"].Content = new Bishop(GameID, 26, true, new Position(41, "f", "1"), GameState);
            Fields["c8"].Content = new Bishop(GameID, 27, false, new Position(24, "c", "8"), GameState);     // set black bishops
            Fields["f8"].Content = new Bishop(GameID, 28, false, new Position(48, "f", "8"), GameState);
            Fields["d1"].Content = new Queen(GameID, 29, true, new Position(25, "d", "1"), GameState);       // set white queen
            Fields["d8"].Content = new Queen(GameID, 30, false, new Position(32, "d", "8"), GameState);      // set black queen
            Fields["e1"].Content = new King(GameID, 31, true, new Position(33, "e", "1"), GameState);        // set white king
            GameState.WhiteKing = (King)Fields["e1"].Content;
            Fields["e8"].Content = new King(GameID, 32, false, new Position(40, "e", "8"), GameState);       // set black king
            GameState.BlackKing = (King)Fields["e8"].Content;
        }

        private void RefreshAttackedSquares()
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

        private void MakeAMove()
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
                ClonePieceAndNewPositionPiece(move.NewPosition.Name, piece, ref pieceCloned, ref newPositionContentCloned, Fields);
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
        private void GetInputAndValidateIt(ref string chosenMove, ref Move move)
        {
            Console.Write($" {GameState.CurrentPlayer} turn, make a move: ");  // TO DO: frontend action
            chosenMove = Console.ReadLine();
            while (!UserInputIsValid(chosenMove, ref move))
            {
                Console.Write($" It's not a valid input. Type your move in a correct format: ");     // TO DO: frontend action
                chosenMove = Console.ReadLine();
            }
        }

        private bool UserInputIsValid(string chosenMove, ref Move move) //TO DO: rewrite using regex
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

        private Move StringToMove(string str) // str parameter format ex.: "pw e2 e4" or "pw e7 e8 pw" (if its pawn promotion)
        {
            string[] substrings = str.Split(" ");
            int currentPositionId = Chessboard.BoardsPositions.Single(s => s.Position.Name == substrings[1]).PositionID;
            int nextPositionId = Chessboard.BoardsPositions.Single(s => s.Position.Name == substrings[2]).PositionID;
            Move move = new();
            if (str.Length == 8)
            {
                move = new(GameID, substrings[0], currentPositionId, nextPositionId);
            }
            else
            {
                move = new Move(GameID, substrings[0], currentPositionId, nextPositionId, substrings[3]);
            }
            Position currentPosition = new Position(currentPositionId, substrings[1][0].ToString(), substrings[1][1].ToString());
            Position newPosition = new Position(currentPositionId, substrings[2][0].ToString(), substrings[2][1].ToString());
            move.CurrentPosition = currentPosition;
            move.NewPosition = newPosition;
            return move;
        }

        private void FindPieceAndValidateMove(ref string chosenMove, ref Move move, ref Piece piece)
        {
            piece = FindPiece(move.PieceNameID, move.CurrentPosition.Name);
            while (!MoveIsPossible(piece, move))
            {
                Console.Write($" It's not a valid move. Choose a differnt piece or/and field: "); // TO DO: przepisz pod javascript (migające na czerwono pole, gdy MoveIsPossible = false
                chosenMove = Console.ReadLine();
                move = StringToMove(chosenMove);
                piece = FindPiece(move.PieceNameID, move.CurrentPosition.Name);
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

        private Piece FindPiece(string pieceName, string currentPosition)
        {
            Piece piece = Fields[currentPosition].Content;

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

        private static void ClonePieceAndNewPositionPiece(string newPosition, Piece piece, ref Piece clonedPiece, ref Piece clonedNewPositionContent, Dictionary<string, Field> fields)
        {
            clonedNewPositionContent = fields[newPosition].Content;
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

        private void ChangeBoard(Move move, Piece piece)
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
                    Fields[move.NewPosition.FileID + move.CurrentPosition.RankID].Content = null;
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
                    Position oldPosition = new Position();
                    Position newPosition = new Position();
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
                    rook = (Rook)Fields[oldPosition.Name].Content;   // move a rook
                    rook.Position = newPosition;
                    Fields[newPosition.Name].Content = rook;
                    Fields[oldPosition.Name].Content = null;
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
            Fields[move.NewPosition.Name].Content = Fields[move.CurrentPosition.Name].Content;   // move piece on a new position
            Fields[move.CurrentPosition.Name].Content = null;
        }

        private bool OponentAcceptsADraw()      // TO DO: frontend action
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

        private void UndoBoardChanges(Move move, Piece pieceCloned, Piece newPositionContentCloned)
        {
            Fields[move.CurrentPosition.Name].Content = pieceCloned;   // move piece on a previous position
            Fields[move.NewPosition.Name].Content = newPositionContentCloned;   // restore a previous NewPosition content
        }

        private void PawnPromotion(Move move, int pieceId, bool isWhite)
        {
            switch (move.PromotionTo.PieceNameID)
            {
                case "qw":
                    Fields[move.CurrentPosition.Name].Content = new Queen(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "qb":
                    Fields[move.CurrentPosition.Name].Content = new Queen(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "nw":
                    Fields[move.CurrentPosition.Name].Content = new Knight(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "nb":
                    Fields[move.CurrentPosition.Name].Content = new Knight(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "rw":
                    Fields[move.CurrentPosition.Name].Content = new Rook(GameID, pieceId, isWhite, move.NewPosition, false, GameState);
                    break;
                case "rb":
                    Fields[move.CurrentPosition.Name].Content = new Rook(GameID, pieceId, isWhite, move.NewPosition, false, GameState);
                    break;
                case "bw":
                    Fields[move.CurrentPosition.Name].Content = new Bishop(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
                case "bb":
                    Fields[move.CurrentPosition.Name].Content = new Bishop(GameID, pieceId, isWhite, move.NewPosition, GameState);
                    break;
            }
            Fields[move.CurrentPosition.Name].Content = null;
        }
    }
}

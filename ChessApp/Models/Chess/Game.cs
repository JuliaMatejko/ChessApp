using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
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
        public List<Move> Moves { get; set; } 

        public Game()
        {

        }

        [NotMapped]
        public Dictionary<string, Field> Fields
        {
            get => CreateFieldsDictionary(Chessboard);
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
            
            while (!GameState.IsAWin && !GameState.IsADraw)
            {
                //BoardController.MakeAMove();
                //BoardController.RefreshAttackedSquares();
                if (GameState.PlayersAgreedToADraw)
                {
                    //Console.WriteLine(" Players agreed to a draw.");
                    //Console.WriteLine(" It's a draw!");
                }
                else if (GameState.PlayerResigned)
                {
                    //Console.Write($" {CurrentPlayer} resigned.");
                   // GameState.ChangeTurns();
                    //Console.WriteLine($" {CurrentPlayer} won the game!");
                }
                else if (GameState.IsACheckmate)
                {
                    //BoardView.PrintBoard(Board);

                    //Console.WriteLine(" Checkmate.");
                    //Console.WriteLine($" {CurrentPlayer} won the game!");
                }
                else if (GameState.IsAStalemate)
                {
                    //BoardView.PrintBoard(Board);

                    //Console.WriteLine(" Stalemate.");
                    //Console.WriteLine(" It's a draw!");
                }
                else
                {
                    //BoardView.PrintBoard(Board);
                    GameState.ResetEnPassantFlag();
                    GameState.ResetCurrentPiecesAttackingTheKing();
                    GameState.ChangeTurns();
                }
            }
        }

        public static Dictionary<string, Field> CreateFieldsDictionary(Board board)
        {
            Dictionary<string, Field> fields = new();
            int i = 0;
            int j = 0;
            int fieldAndPositionId = ((i * 8) + j + 1);
            foreach (BoardPosition boardPosition in board.BoardsPositions.Where(s => s.GameID == board.GameID))
            {
                if (j < Board.boardSize)
                {
                    fields[boardPosition.Position.Name] = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == i + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.FieldID == fieldAndPositionId);
                }
                else
                {
                    j = 0;
                    i++;
                    fields[boardPosition.Position.Name] = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == i + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.FieldID == fieldAndPositionId);
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
                Fields[Chessboard.BoardsFiles[i].FileID + "2"].Content = new Pawn(GameID, i + 1, true, new Position((i * 8) + 2, Chessboard.BoardsFiles[i].FileID, "2"));  // set white pawns
            }
            for (var i = 0; i < Board.boardSize; i++)
            {
                Fields[Chessboard.BoardsFiles[i].FileID + "7"].Content = new Pawn(GameID, i + 7, false, new Position((i * 8) + 7, Chessboard.BoardsFiles[i].FileID, "7"));  // set black pawns 
            }
            Fields["a1"].Content = new Rook(GameID, 17, true, new Position(1, "a", "1"));        // set white rooks
            Fields["h1"].Content = new Rook(GameID, 18, true, new Position(57, "h", "1"));
            Fields["a8"].Content = new Rook(GameID, 19, false, new Position(8, "a", "8"));       // set black rooks
            Fields["h8"].Content = new Rook(GameID, 20, false, new Position(64, "h", "8"));
            Fields["b1"].Content = new Knight(GameID, 21, true, new Position(9, "b", "1"));      // set white knights
            Fields["g1"].Content = new Knight(GameID, 22, true, new Position(49, "g", "1"));
            Fields["b8"].Content = new Knight(GameID, 23, false, new Position(16, "b", "8"));     // set black knights
            Fields["g8"].Content = new Knight(GameID, 24, false, new Position(56, "g", "8"));
            Fields["c1"].Content = new Bishop(GameID, 25, true, new Position(17, "c", "1"));      // set white bishops
            Fields["f1"].Content = new Bishop(GameID, 26, true, new Position(41, "f", "1"));
            Fields["c8"].Content = new Bishop(GameID, 27, false, new Position(24, "c", "8"));     // set black bishops
            Fields["f8"].Content = new Bishop(GameID, 28, false, new Position(48, "f", "8"));
            Fields["d1"].Content = new Queen(GameID, 29, true, new Position(25, "d", "1"));       // set white queen
            Fields["d8"].Content = new Queen(GameID, 30, false, new Position(32, "d", "8"));      // set black queen
            Fields["e1"].Content = new King(GameID, 31, true, new Position(33, "e", "1"));        // set white king
            GameState.WhiteKing = (King)Fields["e1"].Content;
            Fields["e8"].Content = new King(GameID, 32, false, new Position(40, "e", "8"));       // set black king
            GameState.BlackKing = (King)Fields["e8"].Content;
        }      
    }
}

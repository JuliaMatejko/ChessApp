using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess.Pieces
{
    public class Pawn : Piece
    {
        [Required]
        public bool IsFirstMove { get; set; } = true;
        [Required]
        public bool CanBeTakenByEnPassantMove { get; set; } = false;

        public Pawn(bool isWhite, Position position)
        {
            IsWhite = isWhite;
            Position = position;
            Name = isWhite ? Name = PieceNames[0] : Name = PieceNames[1];
        }

        public Pawn()
        {

        }
        /*
        protected override HashSet<string> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            StandardPawnMove(fileIndex, rankIndex, board, positions);
            EnPassantPawnMove(fileIndex, rankIndex, positions);
            return positions;
        }

        private HashSet<string> StandardPawnMove(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            if (IsWhite)
            {
                if (IsFirstMove)
                {
                    MoveTwoForward();
                }
                if (rankIndex < Board.boardSize - 1)
                {
                    MoveOneForward();
                    switch (fileIndex)
                    {
                        case 0:
                            MoveOneForwardDiagonallyRight();
                            break;
                        case 7:
                            MoveOneForwardDiagonallyLeft();
                            break;
                        default:
                            MoveOneForwardDiagonallyRight();
                            MoveOneForwardDiagonallyLeft();
                            break;
                    }
                }
            }
            else
            {
                if (IsFirstMove)
                {
                    MoveTwoForward();
                }
                if (rankIndex > 0)
                {
                    MoveOneForward();
                    switch (fileIndex)
                    {
                        case 0:
                            MoveOneForwardDiagonallyLeft();
                            break;
                        case 7:
                            MoveOneForwardDiagonallyRight();
                            break;
                        default:
                            MoveOneForwardDiagonallyLeft();
                            MoveOneForwardDiagonallyRight();
                            break;
                    }
                }
            }
            return positions;

            void MoveOneForward() => MovePawn(0, 1, fileIndex, rankIndex, board, positions);
            void MoveTwoForward() => MovePawn(0, 2, fileIndex, rankIndex, board, positions);
            void MoveOneForwardDiagonallyRight() => MovePawn(1, 1, fileIndex, rankIndex, board, positions);
            void MoveOneForwardDiagonallyLeft() => MovePawn(-1, 1, fileIndex, rankIndex, board, positions);
        }

        private HashSet<string> EnPassantPawnMove(int fileIndex, int rankIndex, HashSet<string> positions)
        {
            if (IsWhite)
            {
                if (rankIndex == 4 && Program.Game.BlackPawnThatCanBeTakenByEnPassantMove != null)
                {
                    string oponentPawnFile = Program.Game.BlackPawnThatCanBeTakenByEnPassantMove.Position[0].ToString();
                    if (fileIndex == 0 && oponentPawnFile == Board.Files[fileIndex + 1])
                    {
                        EnPassantRight();
                    }
                    else if (fileIndex == 7 && oponentPawnFile == Board.Files[fileIndex - 1])
                    {
                        EnPassantLeft();
                    }
                    else
                    {
                        if (oponentPawnFile == Board.Files[fileIndex + 1])
                        {
                            EnPassantRight();
                        }
                        if (oponentPawnFile == Board.Files[fileIndex - 1])
                        {
                            EnPassantLeft();
                        }
                    }
                }
            }
            else
            {
                if (rankIndex == 3 && Program.Game.WhitePawnThatCanBeTakenByEnPassantMove != null)
                {
                    string oponentPawnFile = Program.Game.WhitePawnThatCanBeTakenByEnPassantMove.Position[0].ToString();
                    if (fileIndex == 0 && oponentPawnFile == Board.Files[fileIndex + 1])
                    {
                        EnPassantLeft();
                    }
                    else if (fileIndex == 7 && oponentPawnFile == Board.Files[fileIndex - 1])
                    {
                        EnPassantRight();
                    }
                    else
                    {
                        if (oponentPawnFile == Board.Files[fileIndex + 1])
                        {
                            EnPassantLeft();
                        }
                        if (oponentPawnFile == Board.Files[fileIndex - 1])
                        {
                            EnPassantRight();
                        }
                    }
                }
            }
            void EnPassantRight() => EnPassant(1, 1, fileIndex, rankIndex, positions);
            void EnPassantLeft() => EnPassant(-1, 1, fileIndex, rankIndex, positions);

            void EnPassant(int x_white, int y_white, int fileIndex, int rankIndex, HashSet<string> positions)
            {
                int x = IsWhite ? x_white : -x_white;
                int y = IsWhite ? y_white : -y_white;
                positions.Add(Board.Files[fileIndex + x] + Board.Ranks[rankIndex + y]);
            }
            return positions;
        }

        private void MovePawn(int x_white, int y_white, int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            int x = IsWhite ? x_white : -x_white;
            int y = IsWhite ? y_white : -y_white;
            Field newField = board[fileIndex + x][rankIndex + y];

            if (x_white == 0)
            {
                if (y_white == 2)
                {
                    int z = IsWhite ? -1 : 1;
                    if ((board[fileIndex][rankIndex + y + z].Content == null) && (newField.Content == null))
                    {
                        positions.Add(newField.Name);
                    }
                }
                if (y_white == 1)
                {
                    if (newField.Content == null)
                    {
                        positions.Add(newField.Name);
                    }
                }
            }
            if ((x_white == -1 || x_white == 1) && y_white == 1)
            {
                ControlledSquares.Add(newField.Name);

                if (newField.Content != null)
                {
                    bool z = IsWhite ? !(newField.Content.IsWhite) : newField.Content.IsWhite;
                    if (z)
                    {
                        if (newField.Content.GetType() != typeof(King))
                        {
                            positions.Add(newField.Name);
                        }
                        else
                        {
                            if (IsWhite)
                            {
                                Program.Game.BlackKingIsInCheck = true;
                            }
                            else
                            {
                                Program.Game.WhiteKingIsInCheck = true;
                            }
                            Program.Game.CurrentPlayerPiecesAttackingTheKing.Add(this);
                        }
                    }
                }
            }
        }*/
    }
}

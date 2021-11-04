using ChessApp.Models.Chess.BoardProperties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess.Pieces
{
    public class Rook : Piece//, IHorizontallyAndVerticallyMovingPiece
    {
        [Required]
        [DefaultValue(true)]
        public bool IsFirstMove { get; set; } = true;

        public Rook(int gameId, int pieceId, bool isWhite, Position position)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            Position = position;
            PieceNameID = isWhite ? PieceNameID = "rw" : PieceNameID = "rb";
        }

        public Rook(int gameId, int pieceId, bool isWhite, Position position, bool isFirstMove)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            Position = position;
            PieceNameID = isWhite ? PieceNameID = "rw" : PieceNameID = "rb";
            IsFirstMove = isFirstMove;
        }

        public Rook()
        {
                
        }
        /*
        protected override HashSet<string> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            MoveForward(fileIndex, rankIndex, board, positions);
            MoveBackwards(fileIndex, rankIndex, board, positions);
            MoveLeft(fileIndex, rankIndex, board, positions);
            MoveRight(fileIndex, rankIndex, board, positions);
            return positions;
        }

        public void MoveForward(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (rank < Board.boardSize - 1 && canMove)
                {
                    MoveOne(0, 1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (rank > 0 && canMove)
                {
                    MoveOne(0, 1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveBackwards(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (rank > 0 && canMove)
                {
                    MoveOne(0, -1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (rank < Board.boardSize - 1 && canMove)
                {
                    MoveOne(0, -1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveLeft(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (file > 0 && canMove)
                {
                    MoveOne(-1, 0, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (file < Board.boardSize - 1 && canMove)
                {
                    MoveOne(-1, 0, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveRight(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (file < Board.boardSize - 1 && canMove)
                {
                    MoveOne(1, 0, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (file > 0 && canMove)
                {
                    MoveOne(1, 0, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveOne(int x_white, int y_white, ref int file, ref int rank, ref bool canMove, ref bool kingInTheWay, Board board, HashSet<string> positions)
        {
            int x = IsWhite ? x_white : -x_white;
            int y = IsWhite ? y_white : -y_white;
            Field newField = board[file + x][rank + y];

            ControlledSquares.Add(newField.Name);

            if (newField.Content == null)
            {
                if (!kingInTheWay)
                {
                    positions.Add(newField.Name);
                }
                file += x;
                rank += y;
            }
            else
            {
                bool z = IsWhite ? !(newField.Content.IsWhite) : newField.Content.IsWhite;
                if (z)
                {
                    if (newField.Content.GetType() != typeof(King))
                    {
                        if (!kingInTheWay)
                        {
                            positions.Add(newField.Name);
                        }
                        canMove = false;
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

                        kingInTheWay = true;
                        file += x;
                        rank += y;
                    }
                }
                else
                {
                    canMove = false;
                }
            }*/
        }
    }

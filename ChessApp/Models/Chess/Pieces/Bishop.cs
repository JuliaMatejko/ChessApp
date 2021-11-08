using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;
using System.Linq;

namespace ChessApp.Models.Chess.Pieces
{
    public class Bishop : Piece, IDiagonallyMovingPiece
    {
        public Bishop(int gameId, int pieceId, bool isWhite, Position position)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            Position = position;
            PieceNameID = isWhite ? PieceNameID = "bw" : PieceNameID = "bb";
        }

        public Bishop()
        {

        }

        protected override HashSet<NextAvailablePosition> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            MoveRightForward(fileIndex, rankIndex, board, positions);
            MoveLeftBackwards(fileIndex, rankIndex, board, positions);
            MoveLeftForward(fileIndex, rankIndex, board, positions);
            MoveRightBackwards(fileIndex, rankIndex, board, positions);
            return positions;
        }

        public void MoveRightForward(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (file < Board.boardSize - 1 && rank < Board.boardSize - 1 && canMove)
                {
                    MoveOne(1, 1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (file > 0 && rank > 0 && canMove)
                {
                    MoveOne(1, 1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveLeftBackwards(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (file > 0 && rank > 0 && canMove)
                {
                    MoveOne(-1, -1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (file < Board.boardSize - 1 && rank < Board.boardSize - 1 && canMove)
                {
                    MoveOne(-1, -1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveLeftForward(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (file > 0 && rank < Board.boardSize - 1 && canMove)
                {
                    MoveOne(-1, 1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (file < Board.boardSize - 1 && rank > 0 && canMove)
                {
                    MoveOne(-1, 1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveRightBackwards(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            bool canMove = true;
            bool kingInTheWay = false;
            int rank = rankIndex;
            int file = fileIndex;
            if (IsWhite)
            {
                while (file < Board.boardSize - 1 && rank > 0 && canMove)
                {
                    MoveOne(1, -1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
            else
            {
                while (file > 0 && rank < Board.boardSize - 1 && canMove)
                {
                    MoveOne(1, -1, ref file, ref rank, ref canMove, ref kingInTheWay, board, positions);
                }
            }
        }

        public void MoveOne(int x_white, int y_white, ref int file, ref int rank, ref bool canMove, ref bool kingInTheWay, Board board, HashSet<NextAvailablePosition> positions)
        {
            int x = IsWhite ? x_white : -x_white;
            int y = IsWhite ? y_white : -y_white;
            int tempfile = file;
            int fieldAndPositionId = (file + x) * 8 + (rank + y) + 1;
            Piece? content = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == tempfile + x + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.FieldID == fieldAndPositionId).Content;
            int? contentId = content?.PieceID;
            Field newField = new Field(fieldAndPositionId, file + x + 1, fieldAndPositionId, contentId);
            newField.Content = contentId != null ? content : null;
            ControlledSquares.Add(new ControlledSquare(PieceID, newField.PositionID));

            if (newField.Content == null)
            {
                if (!kingInTheWay)
                {
                    positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
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
                            positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
                        }
                        canMove = false;
                    }
                    else
                    {
                        if (IsWhite)
                        {
                            GameState.BlackKingIsInCheck = true;
                        }
                        else
                        {
                            GameState.WhiteKingIsInCheck = true;
                        }
                        GameState.CurrentPlayerPiecesAttackingTheKing.Add(this);

                        kingInTheWay = true;
                        file += x;
                        rank += y;
                    }
                }
                else
                {
                    canMove = false;
                }
            }
        }
    }
}

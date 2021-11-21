using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChessApp.Models.Chess.Pieces
{
    public class Rook : Piece, IHorizontallyAndVerticallyMovingPiece, IFirstMoveMattersPiece
    {
        [Required]
        [DefaultValue(true)]
        public bool IsFirstMove { get; set; }

        public Rook(int gameId, int pieceId, bool isWhite, Field field, GameState gameState)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            PieceNameID = isWhite ? PieceNameID = pieceNames[11] : PieceNameID = pieceNames[10];
            IsFirstMove = true;
            Field = field;
            GameState = gameState;
            GameStateID = gameState.GameID;
        }

        public Rook(int gameId, int pieceId, bool isWhite, bool isFirstMove, Field field, GameState gameState)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            PieceNameID = isWhite ? PieceNameID = pieceNames[11] : PieceNameID = pieceNames[10];
            IsFirstMove = isFirstMove;
            Field = field;
            GameState = gameState;
            GameStateID = gameState.GameID;
        }

        public Rook()
        {

        }

        protected override HashSet<NextAvailablePosition> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            MoveForward(fileIndex, rankIndex, board, positions);
            MoveBackwards(fileIndex, rankIndex, board, positions);
            MoveLeft(fileIndex, rankIndex, board, positions);
            MoveRight(fileIndex, rankIndex, board, positions);
            return positions;
        }

        public void MoveForward(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
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

        public void MoveBackwards(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
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

        public void MoveLeft(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
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

        public void MoveRight(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
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

        public void MoveOne(int x_white, int y_white, ref int file, ref int rank, ref bool canMove, ref bool kingInTheWay, Board board, HashSet<NextAvailablePosition> positions)
        {
            int x = IsWhite ? x_white : -x_white;
            int y = IsWhite ? y_white : -y_white;
            int tempfile = file;
            int fieldAndPositionId = (file + x) * 8 + (rank + y) + 1;
            var content = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == tempfile + x + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldAndPositionId).Content;
            int? contentId = content?.PieceID;
            int? contentGameId = content?.GameID;
            Field newField = new(GameID, GameState.Game.Chessboard.BoardsPositions[fieldAndPositionId - 1].Position, GameState.Game.Chessboard.BoardsFieldColumns[file + x].FieldColumn, contentId, contentGameId)
            {
                Content = contentId != null ? content : null
            };
            ControlledSquares.Add(new ControlledSquare(GameID, PieceID, newField.PositionID));

            if (newField.Content == null)
            {
                if (!kingInTheWay)
                {
                    positions.Add(new NextAvailablePosition(GameID, PieceID, newField.PositionID));
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
                            positions.Add(new NextAvailablePosition(GameID, PieceID, newField.PositionID));
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

using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChessApp.Models.Chess.Pieces
{
    public class King : Piece, IFirstMoveMattersPiece
    {
        [Required]
        [DefaultValue(true)]
        public bool IsFirstMove { get; set; }

        public King(int gameId, int pieceId, bool isWhite, Position position, GameState gameState)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            Position = position;
            PieceNameID = isWhite ? PieceNameID = pieceNames[3]: PieceNameID = pieceNames[2];
            IsFirstMove = true;
            GameState = gameState;
        }

        public King()
        {

        }
        
        protected override HashSet<NextAvailablePosition> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            KingMove(fileIndex, rankIndex, board, positions);
            CastlingMove(fileIndex, rankIndex, board, positions);
            return positions;
        }
        
        private HashSet<NextAvailablePosition> CastlingMove(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            bool kingIsNotInCheck = IsWhite ? !GameState.WhiteKingIsInCheck : !GameState.BlackKingIsInCheck;
            if (IsFirstMove && kingIsNotInCheck)
            {
                var whiteRookR = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == 8)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.Position.RankID == "1").Content;
                var whiteRookL = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.Position.RankID == "1").Content;
                var blackRookR = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == 8)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.Position.RankID == "8").Content;
                var blackRookL = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.Position.RankID == "8").Content;

                Piece[] pieces = IsWhite ? new Piece[] { whiteRookR, whiteRookL }
                                         : new Piece[] { blackRookR, blackRookL };
                if (pieces[0] != null && pieces[0].GetType() == typeof(Rook))
                {
                    Rook rook = (Rook)pieces[0];
                    if (rook.IsFirstMove)
                    {
                        CastleKingSide();
                    }
                }
                if (pieces[1] != null && pieces[1].GetType() == typeof(Rook))
                {
                    Rook rook = (Rook)pieces[1];
                    if (rook.IsFirstMove)
                    {
                        CastleQueenSide();
                    }
                }
            }
            return positions;

            void CastleKingSide() => CastleKingMove(2, fileIndex, rankIndex);
            void CastleQueenSide() => CastleKingMove(-2, fileIndex, rankIndex);

            void CastleKingMove(int x, int file, int rank)
            {
                int z = x == 2 ? 1 : -1;
                int fieldId = (((file + z) * 8) + rank + 1);
                int sfieldId = (((file + x) * 8) + rank + 1);
                Field field = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == file + z + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldId);
                Field sfield = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == file + x + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == sfieldId);

                if (field.Content == null && sfield.Content == null)
                {
                    NextAvailablePosition kingNewPosition = new(PieceID, sfield.PositionID);
                    if (KingNewPositionIsSafe(new NextAvailablePosition(PieceID, field.PositionID), board)
                        && KingNewPositionIsSafe(kingNewPosition, board))
                    {
                        positions.Add(kingNewPosition);
                    }
                }
            }  
        }

        private bool KingNewPositionIsSafe(NextAvailablePosition newPosition, Board board)
        {
            for (var i = 0; i < Board.boardSize; i++)
            {
                for (var j = 0; j < Board.boardSize; j++)
                {
                    int fieldId = ((i * 8) + j + 1);
                    var piece = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == i + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldId).Content;
                    if (piece != null)
                    {
                        bool isOponentsPiece = IsWhite ? !piece.IsWhite : piece.IsWhite;
                        if (isOponentsPiece && piece.ControlledSquares.Contains(new ControlledSquare(piece.PieceID, newPosition.PositionID)))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private HashSet<NextAvailablePosition> KingMove(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            if (rankIndex < Board.boardSize - 1)
            {
                if (IsWhite)
                {
                    MoveOneForward();
                    switch (fileIndex)
                    {
                        case 0:
                            MoveOneRight();
                            MoveOneForwardDiagonallyRight();
                            break;
                        case 7:
                            MoveOneLeft();
                            MoveOneForwardDiagonallyLeft();
                            break;
                        default:
                            MoveOneRight();
                            MoveOneLeft();
                            MoveOneForwardDiagonallyLeft();
                            MoveOneForwardDiagonallyRight();
                            break;
                    }
                }
                else
                {
                    MoveOneBackwards();
                    switch (fileIndex)
                    {
                        case 0:
                            MoveOneLeft();
                            MoveOneBackwardsDiagonallyLeft();
                            break;
                        case 7:
                            MoveOneRight();
                            MoveOneBackwardsDiagonallyRight();
                            break;
                        default:
                            MoveOneRight();
                            MoveOneLeft();
                            MoveOneBackwardsDiagonallyLeft();
                            MoveOneBackwardsDiagonallyRight();
                            break;
                    }
                }
            }
            if (rankIndex > 0)
            {
                if (IsWhite)
                {
                    MoveOneBackwards();
                    switch (fileIndex)
                    {
                        case 0:
                            MoveOneRight();
                            MoveOneBackwardsDiagonallyRight();
                            break;
                        case 7:
                            MoveOneLeft();
                            MoveOneBackwardsDiagonallyLeft();
                            break;
                        default:
                            MoveOneRight();
                            MoveOneLeft();
                            MoveOneBackwardsDiagonallyLeft();
                            MoveOneBackwardsDiagonallyRight();
                            break;
                    }
                }
                else
                {
                    MoveOneForward();
                    switch (fileIndex)
                    {
                        case 0:
                            MoveOneLeft();
                            MoveOneForwardDiagonallyLeft();
                            break;
                        case 7:
                            MoveOneRight();
                            MoveOneForwardDiagonallyRight();
                            break;
                        default:
                            MoveOneRight();
                            MoveOneLeft();
                            MoveOneForwardDiagonallyLeft();
                            MoveOneForwardDiagonallyRight();
                            break;
                    }
                }
            }
            return positions;

            void MoveOneForwardDiagonallyLeft() => MoveKing(-1, 1, fileIndex, rankIndex, board, positions);
            void MoveOneBackwardsDiagonallyLeft() => MoveKing(-1, -1, fileIndex, rankIndex, board, positions);
            void MoveOneForwardDiagonallyRight() => MoveKing(1, 1, fileIndex, rankIndex, board, positions);
            void MoveOneBackwardsDiagonallyRight() => MoveKing(1, -1, fileIndex, rankIndex, board, positions);
            void MoveOneForward() => MoveKing(0, 1, fileIndex, rankIndex, board, positions);
            void MoveOneBackwards() => MoveKing(0, -1, fileIndex, rankIndex, board, positions);
            void MoveOneRight() => MoveKing(1, 0, fileIndex, rankIndex, board, positions);
            void MoveOneLeft() => MoveKing(-1, 0, fileIndex, rankIndex, board, positions);
        }

        private void MoveKing(int x_white, int y_white, int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            int x = IsWhite ? x_white : -x_white;
            int y = IsWhite ? y_white : -y_white;
            int fieldAndPositionId = (fileIndex + x) * 8 + (rankIndex + y) + 1;
            var content = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == fileIndex + x + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldAndPositionId).Content;
            int? contentId = content?.PieceID;
            Field newField = new(fieldAndPositionId, fileIndex + x + 1, contentId);
            newField.Content = contentId != null ? content : null;
            ControlledSquares.Add(new ControlledSquare(PieceID, newField.PositionID));

            if (newField.Content == null)
            {
                if (KingNewPositionIsSafe(new NextAvailablePosition(PieceID, newField.PositionID), board))
                {
                    positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
                }
            }
            else
            {
                bool z = IsWhite ? !(newField.Content.IsWhite) : newField.Content.IsWhite;
                if (z && newField.Content.GetType() != typeof(King))
                {
                    if (KingNewPositionIsSafe(new NextAvailablePosition(PieceID, newField.PositionID), board))
                    {
                        positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
                    }
                }
            }
        }
    }
}

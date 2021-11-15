using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;
using System.Linq;

namespace ChessApp.Models.Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(int gameId, int pieceId, bool isWhite, Position position, GameState gameState)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            Position = position;
            PieceNameID = isWhite ? PieceNameID = pieceNames[5] : PieceNameID = pieceNames[4];
            GameState = gameState;
        }

        public Knight()
        {

        }

        protected override HashSet<NextAvailablePosition> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            KnightMove(fileIndex, rankIndex, board, positions);
            return positions;
        }

        private HashSet<NextAvailablePosition> KnightMove(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            if (rankIndex < Board.boardSize - 2)
            {
                if (IsWhite)
                {
                    switch (fileIndex)
                    {
                        case 0:
                            MoveTwoForwardOneRight();
                            MoveTwoRightOneForward();
                            break;
                        case 1:
                            MoveTwoForwardOneRight();
                            MoveTwoRightOneForward();
                            MoveTwoForwardOneLeft();
                            break;
                        case 6:
                            MoveTwoForwardOneLeft();
                            MoveTwoLeftOneForward();
                            MoveTwoForwardOneRight();
                            break;
                        case 7:
                            MoveTwoForwardOneLeft();
                            MoveTwoLeftOneForward();
                            break;
                        default:
                            MoveTwoForwardOneRight();
                            MoveTwoForwardOneLeft();
                            MoveTwoRightOneForward();
                            MoveTwoLeftOneForward();
                            break;
                    }
                }
                else
                {
                    switch (fileIndex)
                    {
                        case 0:
                            MoveTwoBackwardsOneLeft();
                            MoveTwoLeftOneBackwards();
                            break;
                        case 1:
                            MoveTwoBackwardsOneLeft();
                            MoveTwoLeftOneBackwards();
                            MoveTwoBackwardsOneRight();
                            break;
                        case 6:
                            MoveTwoBackwardsOneRight();
                            MoveTwoRightOneBackwards();
                            MoveTwoBackwardsOneLeft();
                            break;
                        case 7:
                            MoveTwoBackwardsOneRight();
                            MoveTwoRightOneBackwards();
                            break;
                        default:
                            MoveTwoBackwardsOneRight();
                            MoveTwoBackwardsOneLeft();
                            MoveTwoRightOneBackwards();
                            MoveTwoLeftOneBackwards();
                            break;
                    }
                }
            }
            if (rankIndex > 1)
            {
                if (IsWhite)
                {
                    switch (fileIndex)
                    {
                        case 0:
                            MoveTwoBackwardsOneRight();
                            MoveTwoRightOneBackwards();
                            break;
                        case 1:
                            MoveTwoBackwardsOneRight();
                            MoveTwoRightOneBackwards();
                            MoveTwoBackwardsOneLeft();
                            break;
                        case 6:
                            MoveTwoBackwardsOneLeft();
                            MoveTwoLeftOneBackwards();
                            MoveTwoBackwardsOneRight();
                            break;
                        case 7:
                            MoveTwoBackwardsOneLeft();
                            MoveTwoLeftOneBackwards();
                            break;
                        default:
                            MoveTwoBackwardsOneRight();
                            MoveTwoBackwardsOneLeft();
                            MoveTwoRightOneBackwards();
                            MoveTwoLeftOneBackwards();
                            break;
                    }
                }
                else
                {
                    switch (fileIndex)
                    {
                        case 0:
                            MoveTwoForwardOneLeft();
                            MoveTwoLeftOneForward();
                            break;
                        case 1:
                            MoveTwoForwardOneLeft();
                            MoveTwoLeftOneForward();
                            MoveTwoForwardOneRight();
                            break;
                        case 6:
                            MoveTwoForwardOneRight();
                            MoveTwoRightOneForward();
                            MoveTwoForwardOneLeft();
                            break;
                        case 7:
                            MoveTwoForwardOneRight();
                            MoveTwoRightOneForward();
                            break;
                        default:
                            MoveTwoForwardOneRight();
                            MoveTwoForwardOneLeft();
                            MoveTwoRightOneForward();
                            MoveTwoLeftOneForward();
                            break;
                    }
                }
            }
            return positions;

            void MoveTwoForwardOneRight() => MoveKnight(1, 2, fileIndex, rankIndex, board, positions);
            void MoveTwoForwardOneLeft() => MoveKnight(-1, 2, fileIndex, rankIndex, board, positions);
            void MoveTwoBackwardsOneRight() => MoveKnight(1, -2, fileIndex, rankIndex, board, positions);
            void MoveTwoBackwardsOneLeft() => MoveKnight(-1, -2, fileIndex, rankIndex, board, positions);
            void MoveTwoRightOneForward() => MoveKnight(2, 1, fileIndex, rankIndex, board, positions);
            void MoveTwoRightOneBackwards() => MoveKnight(2, -1, fileIndex, rankIndex, board, positions);
            void MoveTwoLeftOneForward() => MoveKnight(-2, 1, fileIndex, rankIndex, board, positions);
            void MoveTwoLeftOneBackwards() => MoveKnight(-2, -1, fileIndex, rankIndex, board, positions);
        }

        private void MoveKnight(int x_white, int y_white, int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
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
                positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
            }
            else
            {
                bool z = IsWhite ? !(newField.Content.IsWhite) : newField.Content.IsWhite;
                if (z)
                {
                    if (newField.Content.GetType() != typeof(King))
                    {
                        positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
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
                    }
                }
            }
        }
    }
}

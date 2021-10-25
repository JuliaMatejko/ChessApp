using ChessApp.Models.Chess.BoardProperties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess.Pieces
{
    public class King : Piece
    {
        [Required]
        [DefaultValue(true)]
        public bool IsFirstMove { get; set; } = true;

        public King(int pieceId, bool isWhite, Position position)
        {
            PieceID = pieceId;
            IsWhite = isWhite;
            Position = position;
            PieceNameID = isWhite ? PieceNameID = "kw" : PieceNameID = "kb";
        }

        public King()
        {

        }
        /*
        protected override HashSet<string> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            KingMove(fileIndex, rankIndex, board, positions);
            CastlingMove(fileIndex, rankIndex, board, positions);
            return positions;
        }

        private HashSet<string> CastlingMove(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            bool kingIsNotInCheck = IsWhite ? !Program.Game.WhiteKingIsInCheck : !Program.Game.BlackKingIsInCheck;
            if (IsFirstMove && kingIsNotInCheck)
            {
                Piece[] pieces = IsWhite ? new Piece[] { board[7][0].Content, board[0][0].Content }
                                         : new Piece[] { board[7][7].Content, board[0][7].Content };
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
                if (board[file + z][rank].Content == null && board[file + x][rank].Content == null)
                {
                    string kingNewPosition = Board.Files[file + x] + Board.Ranks[rank];
                    if (KingNewPositionIsSafe(board[file + z][rank].Name, board)
                        && KingNewPositionIsSafe(kingNewPosition, board))
                    {
                        positions.Add(kingNewPosition);
                    }
                }
            }
        }

        private bool KingNewPositionIsSafe(string newPosition, Board board)
        {
            for (var i = 0; i < Board.boardSize; i++)
            {
                for (var j = 0; j < Board.boardSize; j++)
                {
                    Piece piece = board[i][j].Content;
                    if (piece != null)
                    {
                        bool isOponentsPiece = IsWhite ? !piece.IsWhite : piece.IsWhite;
                        if (isOponentsPiece && piece.ControlledSquares.Contains(newPosition))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private HashSet<string> KingMove(int fileIndex, int rankIndex, Board board, HashSet<string> positions)
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

        private void MoveKing(int x_white, int y_white, int fileIndex, int rankIndex, Board board, HashSet<string> positions)
        {
            int x = IsWhite ? x_white : -x_white;
            int y = IsWhite ? y_white : -y_white;
            Field newField = board[fileIndex + x][rankIndex + y];

            ControlledSquares.Add(newField.Name);

            if (newField.Content == null)
            {
                if (KingNewPositionIsSafe(newField.Name, board))
                {
                    positions.Add(newField.Name);
                }
            }
            else
            {
                bool z = IsWhite ? !(newField.Content.IsWhite) : newField.Content.IsWhite;
                if (z && newField.Content.GetType() != typeof(King))
                {
                    if (KingNewPositionIsSafe(newField.Name, board))
                    {
                        positions.Add(newField.Name);
                    }
                }
            }
        }*/
    }
}

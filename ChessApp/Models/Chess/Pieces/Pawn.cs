using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChessApp.Models.Chess.Pieces
{
    public class Pawn : Piece, IFirstMoveMattersPiece
    {
        [Required]
        [DefaultValue(true)]
        public bool IsFirstMove { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool CanBeTakenByEnPassantMove { get; set; }

        public Pawn(int gameId, int pieceId, bool isWhite, Position position, GameState gameState)
        {
            GameID = gameId;
            PieceID = pieceId;
            IsWhite = isWhite;
            Position = position;
            PieceNameID = isWhite ? PieceNameID = pieceNames[7] : PieceNameID = pieceNames[6];
            IsFirstMove = true;
            CanBeTakenByEnPassantMove = false;
            GameState = gameState;
        }

        public Pawn()
        {

        }
        
        protected override HashSet<NextAvailablePosition> ReturnCorrectPieceMoves(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            StandardPawnMove(fileIndex, rankIndex, board, positions);
            EnPassantPawnMove(fileIndex, rankIndex, positions);
            return positions;
        }

        private HashSet<NextAvailablePosition> StandardPawnMove(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
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

        private HashSet<NextAvailablePosition> EnPassantPawnMove(int fileIndex, int rankIndex, HashSet<NextAvailablePosition> positions)
        {
            if (IsWhite)
            {
                if (rankIndex == 4 && GameState.BlackPawnThatCanBeTakenByEnPassantMove != null)
                {
                    string oponentPawnFile = GameState.BlackPawnThatCanBeTakenByEnPassantMove.Position.FileID;
                    if (fileIndex == 0 && oponentPawnFile == Board.files[fileIndex + 1])
                    {
                        EnPassantRight();
                    }
                    else if (fileIndex == 7 && oponentPawnFile == Board.files[fileIndex - 1])
                    {
                        EnPassantLeft();
                    }
                    else
                    {
                        if (oponentPawnFile == Board.files[fileIndex + 1])
                        {
                            EnPassantRight();
                        }
                        if (oponentPawnFile == Board.files[fileIndex - 1])
                        {
                            EnPassantLeft();
                        }
                    }
                }
            }
            else
            {
                if (rankIndex == 3 && GameState.WhitePawnThatCanBeTakenByEnPassantMove != null)
                {
                    string oponentPawnFile = GameState.WhitePawnThatCanBeTakenByEnPassantMove.Position.FileID;
                    if (fileIndex == 0 && oponentPawnFile == Board.files[fileIndex + 1])
                    {
                        EnPassantLeft();
                    }
                    else if (fileIndex == 7 && oponentPawnFile == Board.files[fileIndex - 1])
                    {
                        EnPassantRight();
                    }
                    else
                    {
                        if (oponentPawnFile == Board.files[fileIndex + 1])
                        {
                            EnPassantLeft();
                        }
                        if (oponentPawnFile == Board.files[fileIndex - 1])
                        {
                            EnPassantRight();
                        }
                    }
                }
            }
            void EnPassantRight() => EnPassant(1, 1, fileIndex, rankIndex, positions);
            void EnPassantLeft() => EnPassant(-1, 1, fileIndex, rankIndex, positions);

            void EnPassant(int x_white, int y_white, int fileIndex, int rankIndex, HashSet<NextAvailablePosition> positions)
            {
                int x = IsWhite ? x_white : -x_white;
                int y = IsWhite ? y_white : -y_white;
                positions.Add(new NextAvailablePosition(PieceID, (fileIndex + x) * 8 + (rankIndex + y) + 1));
            }
            return positions;
        }

        private void MovePawn(int x_white, int y_white, int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions)
        {
            int x = IsWhite ? x_white : -x_white;
            int y = IsWhite ? y_white : -y_white;
            int fieldAndPositionId = (fileIndex + x ) * 8 + (rankIndex + y) + 1;
            var content = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == fileIndex + x + 1)
                                    .FieldColumn.Fields.SingleOrDefault(s => s.PositionID == fieldAndPositionId).Content;
            int? contentId = content?.PieceID; 
            Field newField = new(GameState.Game.Chessboard.BoardsPositions[fieldAndPositionId - 1].Position, fileIndex + x + 1, contentId);
            newField.Content = contentId != null ? content : null;
            if (x_white == 0)
            {
                if (y_white == 2)
                {
                    int z = IsWhite ? -1 : 1;
                    Field secondRowField = board.BoardsFieldColumns.Single(s => s.GameID == board.GameID && s.FieldColumnID == fileIndex + 1)
                                                    .FieldColumn.Fields.Single(s => s.PositionID == (fileIndex * 8) + (rankIndex + y + z) + 1);
                    if ((secondRowField.Content == null) && (newField.Content == null))
                    {
                        positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
                    }
                }
                if (y_white == 1)
                {
                    if (newField.Content == null)
                    {
                        positions.Add(new NextAvailablePosition(PieceID, newField.PositionID));
                    }
                }
            }
            if ((x_white == -1 || x_white == 1) && y_white == 1)
            {
                ControlledSquares.Add(new ControlledSquare(PieceID, newField.PositionID));

                if (newField.Content != null)
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
}

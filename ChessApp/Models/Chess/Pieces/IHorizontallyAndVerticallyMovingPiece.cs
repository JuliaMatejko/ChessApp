using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;

namespace ChessApp.Models.Chess.Pieces
{
    interface IHorizontallyAndVerticallyMovingPiece
    {
        public bool IsWhite { get; set; }
        public HashSet<ControlledSquare> ControlledSquares { get; set; }

        public void MoveForward(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions);

        public void MoveBackwards(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions);

        public void MoveLeft(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions);

        public void MoveRight(int fileIndex, int rankIndex, Board board, HashSet<NextAvailablePosition> positions);

        public void MoveOne(int x_white, int y_white, ref int file, ref int rank, ref bool canMove, ref bool kingInTheWay, Board board, HashSet<NextAvailablePosition> positions);
    }
}

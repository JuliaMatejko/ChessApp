using System.Collections.Generic;

namespace ChessApp.Models.Chess.Pieces
{
    interface IHorizontallyAndVerticallyMovingPiece
    {
        public bool IsWhite { get; set; }
        public HashSet<string> ControlledSquares { get; set; }

        public void MoveForward(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveBackwards(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveLeft(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveRight(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveOne(int x_white, int y_white, ref int file, ref int rank, ref bool canMove, ref bool kingInTheWay, Board board, HashSet<string> positions);
    }
}

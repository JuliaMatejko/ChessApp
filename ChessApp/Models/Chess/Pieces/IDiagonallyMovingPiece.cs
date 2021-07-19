using System.Collections.Generic;

namespace ChessApp.Models.Chess.Pieces
{
    interface IDiagonallyMovingPiece
    {
        public bool IsWhite { get; set; }
        public HashSet<string> ControlledSquares { get; set; }

        public void MoveRightForward(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveLeftBackwards(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveLeftForward(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveRightBackwards(int fileIndex, int rankIndex, Board board, HashSet<string> positions);

        public void MoveOne(int x_white, int y_white, ref int file, ref int rank, ref bool canMove, ref bool kingInTheWay, Board board, HashSet<string> positions);
    }
}

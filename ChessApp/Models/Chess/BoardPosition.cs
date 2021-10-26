using ChessApp.Models.Chess.BoardProperties;

namespace ChessApp.Models.Chess
{
    public class BoardPosition
    {
        public int BoardID { get; set; }
        public int PositionID { get; set; }

        public Board Board { get; set; }
        public Position Position { get; set; }

        public BoardPosition(int boardId, int positionId)
        {
            BoardID = boardId;
            PositionID = positionId;
        }

        public BoardPosition()
        {

        }
    }
}

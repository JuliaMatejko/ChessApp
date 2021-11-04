using ChessApp.Models.Chess.BoardProperties;

namespace ChessApp.Models.Chess
{
    public class BoardPosition
    {
        public int GameID { get; set; }
        public int PositionID { get; set; }

        public Board Board { get; set; }
        public Position Position { get; set; }

        public BoardPosition(int gameId, int positionId)
        {
            GameID = gameId;
            PositionID = positionId;
        }

        public BoardPosition()
        {

        }
    }
}

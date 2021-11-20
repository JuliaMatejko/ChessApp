using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class BoardPosition
    {
        [ForeignKey("Board")]
        public int GameID { get; set; }
        public int PositionID { get; set; }

        public Board Board { get; set; }
        public Position Position { get; set; }

        public BoardPosition(int gameId, Position position)
        {
            GameID = gameId;
            Position = position;
            PositionID = position.PositionID;
        }

        public BoardPosition()
        {

        }
    }
}

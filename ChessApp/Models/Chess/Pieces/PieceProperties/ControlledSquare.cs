using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.Pieces.PieceProperties
{
    public class ControlledSquare
    {
        public int BoardGameID { get; set; }
        public int PieceGameID { get; set; }
        public int PieceID { get; set; }
        public int PositionID { get; set; }

        [ForeignKey("PieceGameID, PieceID")]
        public Piece Piece { get; set; }
        [ForeignKey("PositionID")]
        public Position Position { get; set; }
        [ForeignKey("BoardGameID")]
        public Board Board { get; set; }

        public ControlledSquare(int gameId, int pieceId, int positionId)
        {
            BoardGameID = gameId;
            PieceGameID = gameId;
            PieceID = pieceId;
            PositionID = positionId;
        }

        public ControlledSquare()
        {

        }
    }
}

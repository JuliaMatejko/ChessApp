using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.Pieces.PieceProperties
{
    public class NextAvailablePosition
    {
        public int PieceGameID { get; set; }
        public int PieceID { get; set; }
        public int PositionID { get; set; }

        [ForeignKey("PieceGameID, PieceID")]
        public Piece Piece { get; set; }
        [ForeignKey("PositionID")]
        public Position Position { get; set; }

        public NextAvailablePosition(int gameId, int pieceId, int positionId)
        {
            PieceGameID = gameId;
            PieceID = pieceId;
            PositionID = positionId;
        }

        public NextAvailablePosition()
        {

        }
    }
}

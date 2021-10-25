using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess
{
    public class Move
    {
        public int MoveID { get; set; }
        public string PieceNameID { get; set; }
        public PieceName PieceName { get; set; }
        public int? CurrentPositionID { get; set; }
        public Position CurrentPosition { get; set; }
        public int NewPositionID { get; set; }
        public Position NewPosition { get; set; }
        public string PromotionToID { get; set; }
        public PieceName PromotionTo { get; set; }

        public Move(string pieceNameId, int currentPositionId, int newPositionId)
        {
            PieceNameID = pieceNameId;
            CurrentPositionID = currentPositionId;
            NewPositionID = newPositionId;
        }

        public Move(string pieceNameId, int currentPositionId, int newPositionId, string promotionToId)
        {
            PieceNameID = pieceNameId;
            CurrentPositionID = currentPositionId;
            NewPositionID = newPositionId;
            PromotionToID = promotionToId;
        }

        public Move()
        {

        }
    }
}

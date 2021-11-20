using ChessApp.Models.Chess.Pieces.PieceProperties;

namespace ChessApp.Models.Chess
{
    public class Move
    {
        public int GameID { get; set; }
        public Game Game { get; set; }
        public int MoveID { get; set; }
        public string PieceNameID { get; set; }
        public PieceName PieceName { get; set; }
        public int? CurrentPositionID { get; set; }
        public Position CurrentPosition { get; set; }
        public int NewPositionID { get; set; }
        public Position NewPosition { get; set; }
        public string PromotionToID { get; set; }
        public PieceName PromotionTo { get; set; }

        public Move(int gameId, string pieceNameId, Position currentPosition, Position newPosition)
        {
            GameID = gameId;
            PieceNameID = pieceNameId;
            CurrentPosition = currentPosition;
            CurrentPositionID = currentPosition.PositionID;
            NewPosition = newPosition;
            NewPositionID = newPosition.PositionID;
        }

        public Move(int gameId, string pieceNameId, Position currentPosition, Position newPosition, string promotionToId)
        {
            GameID = gameId;
            PieceNameID = pieceNameId;
            CurrentPosition = currentPosition;
            CurrentPositionID = currentPosition.PositionID;
            NewPosition = newPosition;
            NewPositionID = newPosition.PositionID;
            PromotionToID = promotionToId;
        }

        public Move()
        {

        }
    }
}

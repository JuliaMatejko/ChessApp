using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess
{
    public class Move
    {
        //rewrite? use id instead of piecename?
        [Key]
        public int MoveId { get; set; }
        [Required]
        public PieceName PieceName { get; set; }
        [Required]
        public Position CurrentPosition { get; set; }
        [Required]
        public Position NewPosition { get; set; }
        public PieceName PromotionTo { get; set; } //old string

        public Move(PieceName pieceName, Position currentPosition, Position newPosition)
        {
            PieceName = pieceName;
            CurrentPosition = currentPosition;
            NewPosition = newPosition;
        }

        public Move(PieceName pieceName, Position currentPosition, Position newPosition, PieceName promotionTo)
        {
            PieceName = pieceName;
            CurrentPosition = currentPosition;
            NewPosition = newPosition;
            PromotionTo = promotionTo;
        }

        public Move()
        {

        }
    }
}

using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class Field
    {
        [Key]
        public int PositionID { get; set; }
        public Position Position { get; set; }
        [Column("ContentID")]
        public int? PieceID { get; set; }
        public Piece Content { get; set; }

        public int FieldColumnID { get; set; }
        public FieldColumn FieldColumn { get; set; }

        public Field(Position position, int fieldColumnId, int? contentId)
        {
            Position = position;
            PositionID = position.PositionID;
            FieldColumnID = fieldColumnId;
            PieceID = contentId;
        }

        public Field()
        {

        }
    }
}
using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FieldID { get; set; }
        
        [Required]
        public int PositionID { get; set; }
        public Position Position { get; set; }

#nullable enable
        [Column("ContentID")]
        public int? PieceID { get; set; }
        public Piece? Content { get; set; }
#nullable disable

        public int FieldColumnID { get; set; }
        public FieldColumn FieldColumn { get; set; }

        public Field(int fieldId, int fieldColumnId, int positionId, int? contentId)
        {
            FieldID = fieldId;
            FieldColumnID = fieldColumnId;
            PositionID = positionId;
            PieceID = contentId;
        }

        public Field()
        {

        }
    }
}
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

        [Required]
        public int FieldColumnID { get; set; }

        public Field(Position position, Piece content)
        {
            Position = position;
            Content = content;
        }

        public Field()
        {

        }
    }
}
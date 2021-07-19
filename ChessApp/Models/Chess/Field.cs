using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class Field
    {
        [Key]
        public int FieldId { get; set; }
        [Required]
        public Position Position { get; set; }
        public Piece Content { get; set; }

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
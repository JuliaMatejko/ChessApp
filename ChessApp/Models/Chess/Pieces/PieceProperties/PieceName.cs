using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.Pieces.PieceProperties
{
    public class PieceName
    {
        [Key]
        public int PieceNameId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(2)")]
        public string Name { get; set; }

        public PieceName(string name)
        {
            Name = name;
        }
    }
}
